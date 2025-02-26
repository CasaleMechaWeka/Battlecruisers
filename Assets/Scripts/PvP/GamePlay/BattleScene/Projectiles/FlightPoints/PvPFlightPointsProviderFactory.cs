using BattleCruisers.Projectiles.FlightPoints;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public class PvPFlightPointsProviderFactory : IPvPFlightPointsProviderFactory
    {
        public IFlightPointsProvider RocketFlightPointsProvider { get; }
        public IFlightPointsProvider InaccurateRocketFlightPointsProvider { get; }
        public IFlightPointsProvider NukeFlightPointsProvider { get; }

        public PvPFlightPointsProviderFactory()
        {
            RocketFlightPointsProvider = new PvPRocketFlightPointsProvider();
            InaccurateRocketFlightPointsProvider
                = new PvPInaccuratyRocketFlightPointsProvider(
                    new PvPFlightPointStats(
                        ascendPointRadiusVariationM: 1,
                        descendPointRadiusVariationM: 4,
                        targetPointXRadiusVariationM: 10));
            NukeFlightPointsProvider = new PvPNukeFlightPointsProvider();
        }
    }
}