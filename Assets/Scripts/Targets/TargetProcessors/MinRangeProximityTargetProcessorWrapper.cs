using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class MinRangeProximityTargetProcessorWrapper : TargetProcessorWrapper
    {
        private ITargetFinder _targetFinder;

        protected override ITargetProcessor CreateTargetProcessor(
            ITargetsFactory targetsFactory, 
            Faction enemyFaction, 
            IList<TargetType> attackCapabilities, 
            float maxRangeInM, 
            float minRangeInM)
        {
            Assert.IsTrue(maxRangeInM > minRangeInM);

			CircleTargetDetector maxRangeDetector = transform.FindNamedComponent<CircleTargetDetector>("MaxRangeDetector");
            maxRangeDetector.Initialise(maxRangeInM);

            CircleTargetDetector minRangeDetector = transform.FindNamedComponent<CircleTargetDetector>("MinRangeDetector");
            minRangeDetector.Initialise(minRangeInM);

            // Create target finder
            ITargetFilter enemyDetectionFilter = targetsFactory.CreateTargetFilter(enemyFaction, attackCapabilities);
            _targetFinder = targetsFactory.CreateMinRangeTargetFinder(maxRangeDetector, minRangeDetector, enemyDetectionFilter);

            // Start processing targets
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
