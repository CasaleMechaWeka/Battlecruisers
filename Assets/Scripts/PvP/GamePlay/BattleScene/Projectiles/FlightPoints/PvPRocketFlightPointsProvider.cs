using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints
{
    public class PvPRocketFlightPointsProvider : IPvPFlightPointsProvider
    {
        private const float ROCKET_CRUISING_POINTS_OFFSET_PROPORTION = 0.25f;
        private const float ROCKET_MIN_HORIZONTAL_DISTANCE_IN_M = 10;

        /// <summary>
        /// Determine rocket ascent and descent points, and set current target point as the ascent point.
        /// 
        /// Assumes the target does not move.
        /// </summary>
        public Queue<Vector2> FindFlightPoints(Vector2 sourcePosition, Vector2 targetPosition, float cruisingAltitudeInM)
        {
            float horizontalDistanceToTarget = Mathf.Abs(sourcePosition.x - targetPosition.x);
            Assert.IsTrue(horizontalDistanceToTarget >= ROCKET_MIN_HORIZONTAL_DISTANCE_IN_M);
            float cruisingPointsXOffset = ROCKET_CRUISING_POINTS_OFFSET_PROPORTION * horizontalDistanceToTarget;

            Queue<Vector2> flightPoints = new Queue<Vector2>();

            flightPoints.Enqueue(CreateAscendPoint(sourcePosition, targetPosition, cruisingPointsXOffset, cruisingAltitudeInM));
            flightPoints.Enqueue(CreateDescendPoint(sourcePosition, targetPosition, cruisingPointsXOffset, cruisingAltitudeInM));
            flightPoints.Enqueue(CreateTargetPoint(targetPosition));

            return flightPoints;
        }

        protected virtual Vector2 CreateAscendPoint(Vector2 sourcePosition, Vector2 targetPosition, float cruisingPointsXOffset, float cruisingAltitudeInM)
        {
            if (sourcePosition.x < targetPosition.x)
            {
                return new Vector2(sourcePosition.x + cruisingPointsXOffset, cruisingAltitudeInM);
            }
            else
            {
                return new Vector2(sourcePosition.x - cruisingPointsXOffset, cruisingAltitudeInM);
            }
        }

        protected virtual Vector2 CreateDescendPoint(Vector2 sourcePosition, Vector2 targetPosition, float cruisingPointsXOffset, float cruisingAltitudeInM)
        {
            if (sourcePosition.x < targetPosition.x)
            {
                return new Vector2(targetPosition.x - cruisingPointsXOffset, cruisingAltitudeInM);
            }
            else
            {
                return new Vector2(targetPosition.x + cruisingPointsXOffset, cruisingAltitudeInM);
            }
        }

        protected virtual Vector2 CreateTargetPoint(Vector2 targetPosition)
        {
            return targetPosition;
        }
    }
}
