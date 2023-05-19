using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders
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
    public class PvPGlobalTargetFinder : IPvPTargetFinder
    {
        private IPvPCruiser _enemyCruiser;

        private const float BUILD_PROGRESS_CONSIDERED_TARGET = 0.5f;

        public event EventHandler<PvPTargetEventArgs> TargetFound;
        public event EventHandler<PvPTargetEventArgs> TargetLost;

        public PvPGlobalTargetFinder(IPvPCruiser enemyCruiser)
        {
            Assert.IsNotNull(enemyCruiser);
            Assert.IsFalse(enemyCruiser.IsDestroyed);

            _enemyCruiser = enemyCruiser;

            _enemyCruiser.Destroyed += _enemyCruiser_Destroyed;
            _enemyCruiser.BuildingStarted += _enemyCruiser_BuildingStarted;
        }

        private void _enemyCruiser_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            InvokeTargetLostEvent(_enemyCruiser);
        }

        private void _enemyCruiser_BuildingStarted(object sender, PvPBuildingStartedEventArgs e)
        {
            IPvPBuilding building = e.StartedBuilding;

            building.BuildableProgress += Buildable_BuildableProgress;
            building.Destroyed += Building_Destroyed;
        }

        private void Buildable_BuildableProgress(object sender, PvPBuildProgressEventArgs e)
        {
            if (e.Buildable.BuildProgress >= BUILD_PROGRESS_CONSIDERED_TARGET)
            {
                e.Buildable.BuildableProgress -= Buildable_BuildableProgress;
                InvokeTargetFoundEvent(e.Buildable);
            }
        }

        private void Building_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            e.DestroyedTarget.Parse<IPvPBuildable>().BuildableProgress -= Buildable_BuildableProgress;
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

            IPvPBuilding building = e.DestroyedTarget.Parse<IPvPBuilding>();

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

        private void InvokeTargetFoundEvent(IPvPTarget targetFound)
        {
            // Logging.Log(Tags.GLOBAL_TARGET_FINDER, $"Found target: {targetFound}  Target faction: {targetFound.Faction}  Enemy cruiser faction: {_enemyCruiser.Faction}");
            TargetFound?.Invoke(this, new PvPTargetEventArgs(targetFound));
        }

        private void InvokeTargetLostEvent(IPvPTarget targetLost)
        {
            // Logging.Log(Tags.GLOBAL_TARGET_FINDER, $"Lost target: {targetLost}  Target faction: {targetLost.Faction}  Enemy cruiser faction: {_enemyCruiser.Faction}");
            TargetLost?.Invoke(this, new PvPTargetEventArgs(targetLost));
        }

        public override string ToString()
        {
            return base.ToString() + $"  Faction: {PvPHelper.GetOppositeFaction(_enemyCruiser.Faction)}";
        }

        public void DisposeManagedState()
        {
            _enemyCruiser.Destroyed -= _enemyCruiser_Destroyed;
            _enemyCruiser.BuildingStarted -= _enemyCruiser_BuildingStarted;
        }
    }
}
