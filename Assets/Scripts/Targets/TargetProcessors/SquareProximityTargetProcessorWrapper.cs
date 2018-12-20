using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class SquareProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        protected override ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
            TargetDetectorController enemyDetector = GetComponentInChildren<TargetDetectorController>();
            Assert.IsNotNull(enemyDetector);
            enemyDetector.Initialise();

            ITargetFilter enemyDetectionFilter = args.TargetsFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetsFactory.CreateRangedTargetFinder(enemyDetector, enemyDetectionFilter);
        }
    }
}
