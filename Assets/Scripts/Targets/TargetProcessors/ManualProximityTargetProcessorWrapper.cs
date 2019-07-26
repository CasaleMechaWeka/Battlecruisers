using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ManualProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        // Hold reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private ManualDetectorPoller _detectorPoller;
#pragma warning restore CS0414  // Variable is assigned but never used

        protected override ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
            Assert.IsNull(_detectorPoller, "Should only be called once.");

            IManualProximityTargetDetector proximityTargetDetector = args.TargetFactories.TargetDetectorFactory.CreateEnemyShipTargetDetector(args.ParentTarget.Transform, args.MaxRangeInM);
            _detectorPoller = args.TargetFactories.TargetDetectorFactory.CreateManualDetectorPoller(proximityTargetDetector);

            ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetFactories.FinderFactory.CreateRangedTargetFinder(proximityTargetDetector, enemyDetectionFilter);
        }
    }
}
