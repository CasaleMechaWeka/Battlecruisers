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
        private readonly IPvPCruiserSpecificFactories _cruiserSpecificFactories;

        public PvPTargetProviderFactory(IPvPCruiserSpecificFactories cruiserSpecificFactories)
        {
            PvPHelper.AssertIsNotNull(cruiserSpecificFactories);

            _cruiserSpecificFactories = cruiserSpecificFactories;
        }

        public ITargetProvider CreateStaticTargetProvider(ITarget target)
        {
            return new StaticTargetProvider(target);
        }

        public IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingEnemyProvider(_cruiserSpecificFactories, enemyDetector, parentUnit);
        }

        public IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IPvPUnit parentUnit)
        {
            return new PvPShipBlockingFriendlyProvider(friendlyDetector, parentUnit);
        }
    }
}
