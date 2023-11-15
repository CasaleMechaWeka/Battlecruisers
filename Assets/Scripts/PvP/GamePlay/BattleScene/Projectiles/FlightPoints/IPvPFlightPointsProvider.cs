using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public interface IPvPFlightPointsProvider
    {
        Queue<Vector2> FindFlightPoints(Vector2 sourcePosition, Vector2 targetPosition, float cruisingAltitudeInM);
    }
}