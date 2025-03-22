using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ManualProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper
    {
        private ManualDetectorProvider _manualDetectorProvider;

        protected override ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
            Assert.IsNull(_manualDetectorProvider, "Should only be called once.");

            _manualDetectorProvider
                = args.CruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipTargetDetector(
                    args.ParentTarget.Transform,
                    args.MaxRangeInM,
                    args.TargetFactories.RangeCalculatorProvider.BasicCalculator);
            ITargetFilter enemyDetectionFilter = TargetFilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return new RangedTargetFinder(_manualDetectorProvider.TargetDetector, enemyDetectionFilter);
        }

        public override void DisposeManagedState()
        {
            base.DisposeManagedState();

            _manualDetectorProvider?.DisposeManagedState();
            _manualDetectorProvider = null;
        }
    }
}
