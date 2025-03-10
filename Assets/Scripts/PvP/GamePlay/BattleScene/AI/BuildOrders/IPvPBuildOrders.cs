namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public interface IPvPBuildOrders
    {
        IPvPDynamicBuildOrder OffensiveBuildOrder { get; }
        IPvPDynamicBuildOrder AntiAirBuildOrder { get; }
        IPvPDynamicBuildOrder AntiNavalBuildOrder { get; }
    }
}