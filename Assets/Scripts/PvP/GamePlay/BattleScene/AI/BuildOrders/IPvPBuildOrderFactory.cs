using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public interface IPvPBuildOrderFactory
    {
        IPvPDynamicBuildOrder CreateBasicBuildOrder(ILevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAdaptiveBuildOrder(ILevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAntiAirBuildOrder(ILevelInfo levelInfo);
        IPvPDynamicBuildOrder CreateAntiNavalBuildOrder(ILevelInfo levelInfo);
        bool IsAntiRocketBuildOrderAvailable();
        IPvPDynamicBuildOrder CreateAntiRocketBuildOrder();
        bool IsAntiStealthBuildOrderAvailable();
        IPvPDynamicBuildOrder CreateAntiStealthBuildOrder();
    }
}
