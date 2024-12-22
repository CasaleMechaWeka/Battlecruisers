using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public interface IPvPBuildOrderFactory
    {
        IPvPDynamicBuildOrder CreateBasicBuildOrder(IPvPLevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAdaptiveBuildOrder(IPvPLevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAntiAirBuildOrder(IPvPLevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAntiNavalBuildOrder(IPvPLevelInfo levelInfo);
        bool IsAntiRocketBuildOrderAvailable(IPvPLevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAntiRocketBuildOrder();
        bool IsAntiStealthBuildOrderAvailable(IPvPLevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAntiStealthBuildOrder();
    }
}
