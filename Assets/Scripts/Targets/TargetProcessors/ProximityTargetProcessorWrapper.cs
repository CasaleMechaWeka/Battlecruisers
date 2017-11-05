using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ProximityTargetProcessorWrapper : TargetProcessorWrapper
	{
		private ITargetFinder _targetFinder;

        protected override ITargetProcessor CreateTargetProcessor(
            ITargetsFactory targetsFactory, 
            Faction enemyFaction, 
            float detectionRangeInM, 
            IList<TargetType> attackCapabilities)
		{
			CircleTargetDetector enemyDetector = gameObject.GetComponentInChildren<CircleTargetDetector>();
			Assert.IsNotNull(enemyDetector);

            // Create target finder
            enemyDetector.Initialise(detectionRangeInM);
			ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, attackCapabilities);
			_targetFinder = targetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);

			// Create target processor
			ITargetRanker targetRanker = targetsFactory.CreateEqualTargetRanker();
			return targetsFactory.CreateTargetProcessor(_targetFinder, targetRanker);
		}

        protected override void CleanUp()
        {
            base.CleanUp();
            _targetFinder.Dispose();
        }
    }
}
