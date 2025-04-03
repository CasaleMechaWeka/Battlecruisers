using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    public class PvPSquareProximityTargetProcessorWrapper : PvPProximityTargetProcessorWrapper
    {
        protected override ITargetFinder CreateTargetFinder(IPvPTargetProcessorArgs args)
        {
            PvPTargetDetectorController enemyDetector = GetComponentInChildren<PvPTargetDetectorController>();
            Assert.IsNotNull(enemyDetector);

            ITargetFilter enemyDetectionFilter = PvPTargetFactoriesProvider.FilterFactory.CreateTargetFilter(args.EnemyFaction, args.AttackCapabilities);
            return new RangedTargetFinder(enemyDetector, enemyDetectionFilter);
        }
    }
}
