namespace BattleCruisers.AI.BuildOrders
{
    public class BuildOrders : IBuildOrders
	{
        public IDynamicBuildOrder OffensiveBuildOrder { get; }
        public IDynamicBuildOrder AntiAirBuildOrder { get; }
        public IDynamicBuildOrder AntiNavalBuildOrder { get; }

        public BuildOrders(
            IDynamicBuildOrder offensiveBuildOrder,
	        IDynamicBuildOrder antiAirBuildOrder,
	        IDynamicBuildOrder antiNavalBuildOrder)
        {
            OffensiveBuildOrder = offensiveBuildOrder;
            AntiAirBuildOrder = antiAirBuildOrder;
            AntiNavalBuildOrder = antiNavalBuildOrder;
		}
	}
}
