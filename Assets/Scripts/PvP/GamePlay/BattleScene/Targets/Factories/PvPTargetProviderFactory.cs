using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetDetectors;

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

        public IPvPTargetProvider CreateStaticTargetProvider(ITarget target)
        {
            return new PvPStaticTargetProvider(target);
        }

        public IPvPBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingEnemyProvider(_cruiserSpecificFactories, _targetFactoriesProvider, enemyDetector, parentUnit);
        }

        public IPvPBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingFriendlyProvider(_targetFactoriesProvider, friendlyDetector, parentUnit);
        }
    }
}
