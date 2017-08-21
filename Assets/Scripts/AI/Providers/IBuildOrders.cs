namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrders
    {
		IDynamicBuildOrder OffensiveBuildOrder { get; }
		IDynamicBuildOrder AntiAirBuildOrder { get; }
		IDynamicBuildOrder AntiNavalBuildOrder { get; }
	}
}