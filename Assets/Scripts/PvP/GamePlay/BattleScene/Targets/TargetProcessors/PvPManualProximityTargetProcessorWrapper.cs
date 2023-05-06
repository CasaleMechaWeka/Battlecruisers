using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPManualProximityTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        private PvPManualDetectorProvider _manualDetectorProvider;

        protected override IPvPTargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            Assert.IsNull(_manualDetectorProvider, "Should only be called once.");

            _manualDetectorProvider
                = args.CruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipTargetDetector(
                    args.ParentTarget.Transform,
                    args.MaxRangeInM,
                    args.TargetFactories.RangeCalculatorProvider.BasicCalculator);
            IPvPTargetFilter enemyDetectionFilter = args.TargetFactories.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return args.TargetFactories.FinderFactory.CreateRangedTargetFinder(_manualDetectorProvider.TargetDetector, enemyDetectionFilter);
        }

        public override void DisposeManagedState()
        {
            base.DisposeManagedState();

            _manualDetectorProvider?.DisposeManagedState();
            _manualDetectorProvider = null;
        }
    }
}
