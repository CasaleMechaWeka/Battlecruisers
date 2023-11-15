namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPBuildOrders : IPvPBuildOrders
    {
        public IPvPDynamicBuildOrder OffensiveBuildOrder { get; }
        public IPvPDynamicBuildOrder AntiAirBuildOrder { get; }
        public IPvPDynamicBuildOrder AntiNavalBuildOrder { get; }

        public PvPBuildOrders(
            IPvPDynamicBuildOrder offensiveBuildOrder,
            IPvPDynamicBuildOrder antiAirBuildOrder,
            IPvPDynamicBuildOrder antiNavalBuildOrder)
        {
            OffensiveBuildOrder = offensiveBuildOrder;
            AntiAirBuildOrder = antiAirBuildOrder;
            AntiNavalBuildOrder = antiNavalBuildOrder;
        }
    }
}
