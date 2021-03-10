using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.FlightPoints
{
	public class RocketFlightPointsProvider : IFlightPointsProvider
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
			Queue<Vector2> flightPoints = new Queue<Vector2>();

			float horizontalDistanceToTarget = Mathf.Abs(sourcePosition.x - targetPosition.x);
			Assert.IsTrue(horizontalDistanceToTarget >= ROCKET_MIN_HORIZONTAL_DISTANCE_IN_M);

			float cruisingPointsXOffset = ROCKET_CRUISING_POINTS_OFFSET_PROPORTION * horizontalDistanceToTarget;

			if (sourcePosition.x < targetPosition.x)
			{
				flightPoints.Enqueue(new Vector2(sourcePosition.x + cruisingPointsXOffset, cruisingAltitudeInM));
				flightPoints.Enqueue(new Vector2(targetPosition.x - cruisingPointsXOffset, cruisingAltitudeInM));
			}
			else
			{
				flightPoints.Enqueue(new Vector2(sourcePosition.x - cruisingPointsXOffset, cruisingAltitudeInM));
				flightPoints.Enqueue(new Vector2(targetPosition.x + cruisingPointsXOffset, cruisingAltitudeInM));
			}

			flightPoints.Enqueue(targetPosition);

			return flightPoints;
		}
	}
}
