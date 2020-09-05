namespace BattleCruisers.AI.BuildOrders
{
    public interface IBuildOrderFactory
    {
        IDynamicBuildOrder CreateBasicBuildOrder(ILevelInfo levelInfo);
        IDynamicBuildOrder CreateAdaptiveBuildOrder(ILevelInfo levelInfo);
		IDynamicBuildOrder CreateAntiAirBuildOrder(ILevelInfo levelInfo);
        IDynamicBuildOrder CreateAntiNavalBuildOrder(ILevelInfo levelInfo);
        bool IsAntiRocketBuildOrderAvailable();
        IDynamicBuildOrder CreateAntiRocketBuildOrder();
        bool IsAntiStealthBuildOrderAvailable();
        IDynamicBuildOrder CreateAntiStealthBuildOrder();
	}
}
