using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public interface IPvPTargetProviderFactory
    {
        IPvPTargetProvider CreateStaticTargetProvider(IPvPTarget target);
        IPvPBroadcastingTargetProvider CreateShipBlockingEnemyProvider(IPvPTargetDetector enemyDetector, IPvPUnit parentUnit);
        IPvPBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(IPvPTargetDetector friendlyDetector, IPvPUnit parentUnit);
    }
}
