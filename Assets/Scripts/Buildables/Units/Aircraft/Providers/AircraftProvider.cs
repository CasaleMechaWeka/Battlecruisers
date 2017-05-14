using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Units.Aircraft.Providers
{
	public class SafeZone
	{
		public float MinX { get; private set; }
		public float MaxX { get; private set; }
		public float MinY { get; private set; }
		public float MaxY { get; private set; }

		public SafeZone(float minX, float maxX, float minY, float maxY)
		{
			Assert.IsTrue(minX < maxX);
			Assert.IsTrue(minY < maxY);

			MinX = minX;
			MaxX = maxX;
			MinY = minY;
			MaxY = maxY;
		}
	}

	// FELIX  Also set cruising altitude from provider instead of aircraft knowing about it?
	public interface IAircraftProvider
	{
		SafeZone FighterSafeZone { get; }

		IList<Vector2> FindBomberPatrolPoints(float cruisingAltitude);
		IList<Vector2> FindFighterPatrolPoints(float cruisingAltitude);
	}

	public class AircraftProvider : IAircraftProvider
	{
		private readonly Vector2 _parentCruiserPosition, _enemyCruiserPosition;

		private const float SAFE_ZONE_PARENT_CRUISER_OVERLAP = 10;
		private const float SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE = 25;
		private const float SAFE_ZONE_MIN_Y = 10;
		private const float SAFE_ZONE_MAX_Y = 25;

		private const float FIGHTER_PATROL_MARGIN = 5;
		private const float BOMBER_PATROL_MARGIN = 10;

		public SafeZone FighterSafeZone { get; private set; }

		public AircraftProvider(Vector2 parentCruiserPosition, Vector2 enemyCruiserPosition)
		{
			_parentCruiserPosition = parentCruiserPosition;
			_enemyCruiserPosition = enemyCruiserPosition;

			float minXAdjustment = parentCruiserPosition.x < 0 ? -SAFE_ZONE_PARENT_CRUISER_OVERLAP : SAFE_ZONE_PARENT_CRUISER_OVERLAP;
			float minX = parentCruiserPosition.x + minXAdjustment;

			float maxXAdjustment = enemyCruiserPosition.x < 0 ? SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE : -SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE;
			float maxX = enemyCruiserPosition.x + maxXAdjustment;

			FighterSafeZone = new SafeZone(
				minX: minX,
				maxX: maxX,
				minY: SAFE_ZONE_MIN_Y,
				maxY: SAFE_ZONE_MAX_Y);
		}

		public IList<Vector2> FindBomberPatrolPoints(float cruisingAltitude)
		{
			float parentCruiserPatrolPointAdjustmentX = _parentCruiserPosition.x < 0 ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float parentCruiserPatrolPointX = _parentCruiserPosition.x + parentCruiserPatrolPointAdjustmentX;

			float enemyCruiserPatrolPointAdjustmentX = _enemyCruiserPosition.x < 0 ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float enemyCruiserpatrolPointX = _enemyCruiserPosition.x + enemyCruiserPatrolPointAdjustmentX;

			return new List<Vector2>() 
			{
				new Vector2(parentCruiserPatrolPointX, cruisingAltitude),
				new Vector2(enemyCruiserpatrolPointX, cruisingAltitude)
			};
		}

		public IList<Vector2> FindFighterPatrolPoints(float cruisingAltitude)
		{
			return new List<Vector2>() 
			{
				new Vector2(FighterSafeZone.MinX + FIGHTER_PATROL_MARGIN, cruisingAltitude),
				new Vector2(FighterSafeZone.MaxX - FIGHTER_PATROL_MARGIN, cruisingAltitude),
			};
		}
	}
}
