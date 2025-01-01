using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetProviderFactory
    {
        IPvPTargetProvider CreateStaticTargetProvider(ITarget target);
        IPvPBroadcastingTargetProvider CreateShipBlockingEnemyProvider(IPvPTargetDetector enemyDetector, IPvPUnit parentUnit);
        IPvPBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(IPvPTargetDetector friendlyDetector, IPvPUnit parentUnit);
    }
}
