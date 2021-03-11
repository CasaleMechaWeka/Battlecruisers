namespace BattleCruisers.Projectiles.FlightPoints
{
	public class FlightPointsProviderFactory : IFlightPointsProviderFactory
	{
		public IFlightPointsProvider RocketFlightPointsProvider { get; }
        public IFlightPointsProvider InaccurateRocketFlightPointsProvider { get; }
		public IFlightPointsProvider NukeFlightPointsProvider { get; }

        public FlightPointsProviderFactory()
		{
			RocketFlightPointsProvider = new RocketFlightPointsProvider();
			InaccurateRocketFlightPointsProvider
				= new InaccuratyRocketFlightPointsProvider(
					new FlightPointStats(
						ascendPointRadiusVariationM: 1,
						descendPointRadiusVariationM: 4,
						targetPointXRadiusVariationM: 10));
			NukeFlightPointsProvider = new NukeFlightPointsProvider();
		}
	}
}