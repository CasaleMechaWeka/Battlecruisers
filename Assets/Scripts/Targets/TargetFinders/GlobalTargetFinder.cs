using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
    /// <summary>
    /// Keeps track of all buildings constructed by the enemy cruiser.
    /// 
    /// When a building reaches 50% completion, it is considered a valid target.
    /// 
    /// This avoids users fooling the AI by starting lots of buildings, but
    /// never committing any resources to them and never completing them.
    /// 
    /// Also has the enemy cruiser as a target.
    /// </summary>
    public class GlobalTargetFinder : ITargetFinder
	{
		private ICruiser _enemyCruiser;

		private const float BUILD_PROGRESS_CONSIDERED_TARGET = 0.5f;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public GlobalTargetFinder(ICruiser enemyCruiser)
		{
            Assert.IsNotNull(enemyCruiser);
            Assert.IsFalse(enemyCruiser.IsDestroyed);

			_enemyCruiser = enemyCruiser;

            _enemyCruiser.Destroyed += _enemyCruiser_Destroyed;
            _enemyCruiser.BuildingStarted += _enemyCruiser_BuildingStarted;
		}

        private void _enemyCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            InvokeTargetLostEvent(_enemyCruiser);
        }

		private void _enemyCruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
		{
			IBuilding building = e.StartedBuilding;

			building.BuildableProgress += Buildable_BuildableProgress;
			building.Destroyed += Building_Destroyed;
		}

		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			if (e.Buildable.BuildProgress >= BUILD_PROGRESS_CONSIDERED_TARGET)
			{
				e.Buildable.BuildableProgress -= Buildable_BuildableProgress;
				InvokeTargetFoundEvent(e.Buildable);
			}
		}

		private void Building_Destroyed(object sender, DestroyedEventArgs e)
		{
			e.DestroyedTarget.Parse<IBuildable>().BuildableProgress -= Buildable_BuildableProgress;
			e.DestroyedTarget.Destroyed -= Building_Destroyed;

			IBuilding building = e.DestroyedTarget.Parse<IBuilding>();

            // Build progress NEVER decreases.  Otherwise there would be a subtle bug:
            // If build progresss went past 50%, but then below 50%
            // TargetLost will never be called for that target.
			if (building.BuildProgress >= BUILD_PROGRESS_CONSIDERED_TARGET)
			{
                InvokeTargetLostEvent(building);
			}
		}

        // Not in constructor because then client code has not had a chance
        // to subribe to our target found event.
        public void EmitCruiserAsGlobalTarget()
        {
            InvokeTargetFoundEvent(_enemyCruiser);
        }

		private void InvokeTargetFoundEvent(ITarget targetFound)
		{
			Logging.Log(Tags.GLOBAL_TARGET_FINDER, $"Found target: {targetFound}  Target faction: {targetFound.Faction}  Enemy cruiser faction: {_enemyCruiser.Faction}");
			TargetFound?.Invoke(this, new TargetEventArgs(targetFound));
		}

        private void InvokeTargetLostEvent(ITarget targetLost)
        {
			Logging.Log(Tags.GLOBAL_TARGET_FINDER, $"Lost target: {targetLost}  Target faction: {targetLost.Faction}  Enemy cruiser faction: {_enemyCruiser.Faction}");
            TargetLost?.Invoke(this, new TargetEventArgs(targetLost));
        }

        public override string ToString()
        {
			return base.ToString() + $"  Faction: {Helper.GetOppositeFaction(_enemyCruiser.Faction)}";
        }

        public void DisposeManagedState()
		{
            _enemyCruiser.Destroyed -= _enemyCruiser_Destroyed;
			_enemyCruiser.BuildingStarted -= _enemyCruiser_BuildingStarted;
		}
	}
}
