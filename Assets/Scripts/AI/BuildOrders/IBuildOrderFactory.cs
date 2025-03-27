namespace BattleCruisers.AI.BuildOrders
{
    public interface IBuildOrderFactory
    {
        IDynamicBuildOrder CreateBasicBuildOrder(LevelInfo levelInfo);
        IDynamicBuildOrder CreateAdaptiveBuildOrder(LevelInfo levelInfo);
        IDynamicBuildOrder CreateAntiAirBuildOrder(LevelInfo levelInfo);
        IDynamicBuildOrder CreateAntiNavalBuildOrder(LevelInfo levelInfo);
        bool IsAntiRocketBuildOrderAvailable();
        IDynamicBuildOrder CreateAntiRocketBuildOrder();
        bool IsAntiStealthBuildOrderAvailable();
        IDynamicBuildOrder CreateAntiStealthBuildOrder();
    }
}
