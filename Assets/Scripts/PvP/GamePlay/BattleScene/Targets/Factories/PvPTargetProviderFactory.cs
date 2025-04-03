using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetProviderFactory : IPvPTargetProviderFactory
    {
        private readonly PvPTargetFactoriesProvider _targetFactoriesProvider;
        private readonly IPvPCruiserSpecificFactories _cruiserSpecificFactories;

        public PvPTargetProviderFactory(IPvPCruiserSpecificFactories cruiserSpecificFactories, PvPTargetFactoriesProvider targetFactoriesProvider)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories, targetFactoriesProvider);

            _cruiserSpecificFactories = cruiserSpecificFactories;
            _targetFactoriesProvider = targetFactoriesProvider;
        }

        public ITargetProvider CreateStaticTargetProvider(ITarget target)
        {
            return new StaticTargetProvider(target);
        }

        public IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingEnemyProvider(_cruiserSpecificFactories, _targetFactoriesProvider, enemyDetector, parentUnit);
        }

        public IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingFriendlyProvider(_targetFactoriesProvider, friendlyDetector, parentUnit);
        }
    }
}
