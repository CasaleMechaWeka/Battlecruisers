using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.ThreatMonitors
{
    /// <summary>
    /// Monitors a specific building type.
    /// 
    /// Evaluates the threat level when:
    /// + A building is 50% complete
    /// + A building is completed
    /// + A building is destroyed
    /// </summary>
    public class BuildingThreatMonitor<TBuilding> : ImmediateThreatMonitor where TBuilding : class, IBuilding
    {
        private readonly IList<TBuilding> _buildings;

		private const float BUILD_PROGRESS_CONSIDERED_THREAT = 0.5f;

        public BuildingThreatMonitor(ICruiserController enemyCruiser, IThreatEvaluator threatEvaluator)
            : base(enemyCruiser, threatEvaluator)
        {
            _buildings = new List<TBuilding>();

			_enemyCruiser.BuildingStarted += EnemyCruiser_BuildingStarted;
        }

        private void EnemyCruiser_BuildingStarted(object sender, BuildingStartedEventArgs e)
        {
            TBuilding building = e.Buildable as TBuilding;

            if (building != null)
            {
                Assert.IsFalse(_buildings.Contains(building));
                _buildings.Add(building);

                building.BuildableProgress += Building_BuildableProgress;
                building.CompletedBuildable += Building_CompletedBuildable;
                building.Destroyed += Building_Destroyed;
            }
        }

		private void Building_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			if (e.Buildable.BuildProgress >= BUILD_PROGRESS_CONSIDERED_THREAT)
			{
				e.Buildable.BuildableProgress -= Building_BuildableProgress;
                EvaluateThreatLevel();
			}
		}

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            TBuilding completedBuilding = sender.Parse<TBuilding>();
            completedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            EvaluateThreatLevel();
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
		{
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

            TBuilding destroyedBuilding = e.DestroyedTarget.Parse<TBuilding>();
            Assert.IsTrue(_buildings.Contains(destroyedBuilding));

            _buildings.Remove(destroyedBuilding);
            EvaluateThreatLevel();
		}

        protected override float FindThreatEvaluationParameter()
        {
            return _buildings.Sum(building => building.BuildProgress);
        }
    }
}
