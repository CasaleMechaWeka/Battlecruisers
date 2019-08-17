using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetProcessors
{
    public class ManualProximityTargetProcessorWrapper : ProximityTargetProcessorWrapper, IManagedDisposable
    {
        private ManualDetectorProvider _manualDetectorProvider;

        protected override ITargetFinder CreateTargetFinder(ITargetProcessorArgs args)
        {
            Assert.IsNull(_manualDetectorProvider, "Should only be called once.");

            _manualDetectorProvider 
                = args.CruiserSpecificFactories.Targets.TargetDetectorFactory.CreateEnemyShipTargetDetector(
                    args.ParentTarget.Transform,
                    args.MaxRangeInM,
                    args.TargetFactories.RangeCalculatorProvider.BasicCalculator);
            ITargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetFactories.FinderFactory.CreateRangedTargetFinder(_manualDetectorProvider.TargetDetector, enemyDetectionFilter);
        }

        public void DisposeManagedState()
        {
            if (_manualDetectorProvider != null)
            {
                _manualDetectorProvider.DisposeManagedState();
                _manualDetectorProvider = null;
            }
        }
    }
}
