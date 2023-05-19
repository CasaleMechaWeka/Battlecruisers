using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetProviderFactory : IPvPTargetProviderFactory
    {
        private readonly IPvPTargetFactoriesProvider _targetFactoriesProvider;
        private readonly IPvPCruiserSpecificFactories _cruiserSpecificFactories;

        public PvPTargetProviderFactory(IPvPCruiserSpecificFactories cruiserSpecificFactories, IPvPTargetFactoriesProvider targetFactoriesProvider)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories, targetFactoriesProvider);

            _cruiserSpecificFactories = cruiserSpecificFactories;
            _targetFactoriesProvider = targetFactoriesProvider;
        }

        public IPvPTargetProvider CreateStaticTargetProvider(IPvPTarget target)
        {
            return new PvPStaticTargetProvider(target);
        }

        public IPvPBroadcastingTargetProvider CreateShipBlockingEnemyProvider(IPvPTargetDetector enemyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingEnemyProvider(_cruiserSpecificFactories, _targetFactoriesProvider, enemyDetector, parentUnit);
        }

        public IPvPBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(IPvPTargetDetector friendlyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingFriendlyProvider(_targetFactoriesProvider, friendlyDetector, parentUnit);
        }
    }
}
