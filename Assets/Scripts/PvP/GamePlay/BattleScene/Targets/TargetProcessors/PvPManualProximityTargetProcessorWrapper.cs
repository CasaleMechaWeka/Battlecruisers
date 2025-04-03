using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPManualProximityTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        private ManualDetectorProvider _manualDetectorProvider;

        protected override ITargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            Assert.IsNull(_manualDetectorProvider, "Should only be called once.");

            _manualDetectorProvider
                = args.CruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipTargetDetector(
                    args.ParentTarget.Transform,
                    args.MaxRangeInM,
                    PvPTargetFactoriesProvider.RangeCalculatorProvider.BasicCalculator);
            ITargetFilter enemyDetectionFilter = PvPTargetFactoriesProvider.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
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
