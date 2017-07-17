using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.FlightPoints
{
	public interface IFlightPointsProvider
	{
		Queue<Vector2> FindFlightPoints(Vector2 sourcePosition, Vector2 targetPosition, float cruisingAltitudeInM);
	}
}
