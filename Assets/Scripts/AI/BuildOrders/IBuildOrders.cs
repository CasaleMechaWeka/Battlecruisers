namespace BattleCruisers.AI.BuildOrders
{
    public interface IBuildOrders
    {
		IDynamicBuildOrder OffensiveBuildOrder { get; }
		IDynamicBuildOrder AntiAirBuildOrder { get; }
		IDynamicBuildOrder AntiNavalBuildOrder { get; }
	}
}