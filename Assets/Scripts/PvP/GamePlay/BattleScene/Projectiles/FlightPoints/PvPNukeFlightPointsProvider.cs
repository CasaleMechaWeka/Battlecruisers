using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public class PvPNukeFlightPointsProvider : IPvPFlightPointsProvider
    {
        private const float NUKE_MIN_HORIZONTAL_DISTANCE_IN_M = 50;
        private const float NUKE_MIN_CRUISNG_ALTITUDE_IN_M = 25;
        private const float X_INCREMENT_IN_M = 5;
        private const float Y_INCREMENT_IN_M = 5;

        public Queue<Vector2> FindFlightPoints(Vector2 sourcePosition, Vector2 targetPosition, float cruisingAltitudeInM)
        {
            Queue<Vector2> flightPoints = new Queue<Vector2>();

            Assert.IsTrue(cruisingAltitudeInM > NUKE_MIN_CRUISNG_ALTITUDE_IN_M);

            float horizontalDistanceToTarget = Mathf.Abs(sourcePosition.x - targetPosition.x);
            Assert.IsTrue(horizontalDistanceToTarget >= NUKE_MIN_HORIZONTAL_DISTANCE_IN_M);

            float directionMultiplier = sourcePosition.x < targetPosition.x ? 1 : -1;

            flightPoints.Enqueue(new Vector2(sourcePosition.x, cruisingAltitudeInM - 2 * Y_INCREMENT_IN_M));  // -35, 20
            flightPoints.Enqueue(new Vector2(sourcePosition.x + directionMultiplier * X_INCREMENT_IN_M, cruisingAltitudeInM - Y_INCREMENT_IN_M));  // -30, 25
            flightPoints.Enqueue(new Vector2(sourcePosition.x + directionMultiplier * 2 * X_INCREMENT_IN_M, cruisingAltitudeInM));  // -25, 30
            flightPoints.Enqueue(new Vector2(targetPosition.x - directionMultiplier * 3 * X_INCREMENT_IN_M, cruisingAltitudeInM)); // 20, 30
            flightPoints.Enqueue(new Vector2(targetPosition.x - directionMultiplier * 2 * X_INCREMENT_IN_M, cruisingAltitudeInM - Y_INCREMENT_IN_M)); // 25, 25
            flightPoints.Enqueue(new Vector2(targetPosition.x - directionMultiplier * X_INCREMENT_IN_M, cruisingAltitudeInM - 2 * Y_INCREMENT_IN_M)); // 30, 20
            flightPoints.Enqueue(targetPosition);

            return flightPoints;
        }

    }
}
