namespace BattleCruisers.Projectiles.FlightPoints
{
	public class FlightPointsProviderFactory : IFlightPointsProviderFactory
	{
		public IFlightPointsProvider RocketFlightPointsProvider { get; private set; }
		public IFlightPointsProvider NukeFlightPointsProvider { get; private set; }

		public FlightPointsProviderFactory()
		{
			RocketFlightPointsProvider = new RocketFlightPointsProvider();

			// FELIX
			NukeFlightPointsProvider = null;
		}
	}
}