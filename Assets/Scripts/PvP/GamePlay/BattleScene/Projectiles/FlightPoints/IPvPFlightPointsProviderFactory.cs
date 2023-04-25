namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public interface IPvPFlightPointsProviderFactory
    {
        IPvPFlightPointsProvider RocketFlightPointsProvider { get; }
        IPvPFlightPointsProvider InaccurateRocketFlightPointsProvider { get; }
        IPvPFlightPointsProvider NukeFlightPointsProvider { get; }
    }
}
