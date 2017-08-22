namespace BattleCruisers.AI.BuildOrders
{
    public class BuildOrders : IBuildOrders
	{
        public IDynamicBuildOrder OffensiveBuildOrder { get; private set; }
        public IDynamicBuildOrder AntiAirBuildOrder { get; private set; }
        public IDynamicBuildOrder AntiNavalBuildOrder { get; private set; }

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
