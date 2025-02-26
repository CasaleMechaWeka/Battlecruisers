using BattleCruisers.Projectiles.FlightPoints;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public interface IPvPFlightPointsProviderFactory
    {
        IFlightPointsProvider RocketFlightPointsProvider { get; }
        IFlightPointsProvider InaccurateRocketFlightPointsProvider { get; }
        IFlightPointsProvider NukeFlightPointsProvider { get; }
    }
}
