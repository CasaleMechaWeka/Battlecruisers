namespace BattleCruisers.Projectiles.FlightPoints
{
	public interface IFlightPointsProviderFactory
	{
		IFlightPointsProvider RocketFlightPointsProvider { get; }
		IFlightPointsProvider InaccurateRocketFlightPointsProvider { get; }
		IFlightPointsProvider NukeFlightPointsProvider { get; }
	}
}