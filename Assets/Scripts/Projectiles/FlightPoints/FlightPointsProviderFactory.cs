namespace BattleCruisers.Projectiles.FlightPoints
{
	public class FlightPointsProviderFactory : IFlightPointsProviderFactory
	{
		public IFlightPointsProvider RocketFlightPointsProvider { get; }
		public IFlightPointsProvider NukeFlightPointsProvider { get; }

		public FlightPointsProviderFactory()
		{
			RocketFlightPointsProvider = new RocketFlightPointsProvider();
			NukeFlightPointsProvider = new NukeFlightPointsProvider();
		}
	}
}