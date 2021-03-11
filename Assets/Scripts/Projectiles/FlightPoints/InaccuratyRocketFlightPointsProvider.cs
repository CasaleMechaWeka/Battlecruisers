using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.FlightPoints
{
	// FELIX  Test
	// FELIX  Avoid duplicat code with RocketFlightPointsProvider
	/// <summary>
	/// Similar to RocketFlightPointsProvider, but introduces inaccuracy.
	/// </summary>
	public class InaccuratyRocketFlightPointsProvider : IFlightPointsProvider
	{
		private readonly FlightPointStats _stats;
		private readonly IRandomGenerator _random;

		private const float ROCKET_CRUISING_POINTS_OFFSET_PROPORTION = 0.25f;
		private const float ROCKET_MIN_HORIZONTAL_DISTANCE_IN_M = 10;

		// Because may miss target on the x axis, need to aim lower to hit cruiser or water
		// FELIX Test rocket hitting water :P
		private const float TARGET_Y_ADJUSTMENT = -5;

        public InaccuratyRocketFlightPointsProvider(FlightPointStats stats)
        {
			Assert.IsNotNull(stats);

            _stats = stats;
			_random = RandomGenerator.Instance;
        }

        public Queue<Vector2> FindFlightPoints(Vector2 sourcePosition, Vector2 targetPosition, float cruisingAltitudeInM)
		{
			Queue<Vector2> flightPoints = new Queue<Vector2>();

			float horizontalDistanceToTarget = Mathf.Abs(sourcePosition.x - targetPosition.x);
			Assert.IsTrue(horizontalDistanceToTarget >= ROCKET_MIN_HORIZONTAL_DISTANCE_IN_M);

			float cruisingPointsXOffset = ROCKET_CRUISING_POINTS_OFFSET_PROPORTION * horizontalDistanceToTarget;

			Vector2 ascendPoint, descendPoint;

			if (sourcePosition.x < targetPosition.x)
			{
				ascendPoint = new Vector2(sourcePosition.x + cruisingPointsXOffset, cruisingAltitudeInM);
				descendPoint = new Vector2(targetPosition.x - cruisingPointsXOffset, cruisingAltitudeInM);
			}
			else
			{
				ascendPoint = new Vector2(sourcePosition.x - cruisingPointsXOffset, cruisingAltitudeInM);
				descendPoint = new Vector2(targetPosition.x + cruisingPointsXOffset, cruisingAltitudeInM);
			}

			flightPoints.Enqueue(FuzzCruisingPoint(ascendPoint, _stats.AscendPointRadiusVariationM));
			flightPoints.Enqueue(FuzzCruisingPoint(descendPoint, _stats.DescendPointRadiusVariationM));
			flightPoints.Enqueue(FuzzTargetPoint(targetPosition, _stats.TargetPointXRadiusVariationM));

			return flightPoints;
		}
		
		// FELIX Abstract :)
		private Vector2 FuzzCruisingPoint(Vector2 idealPoint, float fuzzRadiusM)
        {
			return
				new Vector2(
					_random.RangeFromCenter(idealPoint.x, fuzzRadiusM),
					_random.RangeFromCenter(idealPoint.y, fuzzRadiusM));
        }

		private Vector2 FuzzTargetPoint(Vector2 targetPoint, float xFuzzRadiusM)
        {
			return
				new Vector2(
					_random.RangeFromCenter(targetPoint.x, xFuzzRadiusM),
					targetPoint.y + TARGET_Y_ADJUSTMENT);
        }
	}
}
