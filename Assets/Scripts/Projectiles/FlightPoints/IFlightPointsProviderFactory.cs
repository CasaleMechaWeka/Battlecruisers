namespace BattleCruisers.Projectiles.FlightPoints
{
	public interface IFlightPointsProviderFactory
	{
		IFlightPointsProvider RocketFlightPointsProvider { get; }
		IFlightPointsProvider NukeFlightPointsProvider { get; }
	}
}