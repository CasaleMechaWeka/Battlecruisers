namespace BattleCruisers.Projectiles.FlightPoints
{
	// FELIX Add inaccuraty rocket provider
	public interface IFlightPointsProviderFactory
	{
		IFlightPointsProvider RocketFlightPointsProvider { get; }
		IFlightPointsProvider NukeFlightPointsProvider { get; }
	}
}