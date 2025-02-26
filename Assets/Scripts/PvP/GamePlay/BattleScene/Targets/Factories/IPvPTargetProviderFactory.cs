using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetProviderFactory
    {
        ITargetProvider CreateStaticTargetProvider(ITarget target);
        IPvPBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IPvPUnit parentUnit);
        IPvPBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IPvPUnit parentUnit);
    }
}
