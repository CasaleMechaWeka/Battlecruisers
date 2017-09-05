using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.Providers
{
    public class AircraftProvider : IAircraftProvider
	{
		private readonly Vector2 _parentCruiserPosition, _enemyCruiserPosition;

		private const float SAFE_ZONE_PARENT_CRUISER_OVERLAP = 10;
		private const float SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE = 25;
		private const float SAFE_ZONE_MIN_Y = 10;
		private const float SAFE_ZONE_MAX_Y = 25;

		private const float FIGHTER_PATROL_MARGIN = 5;
		private const float BOMBER_PATROL_MARGIN = 10;
		private const float DEATHSTAR_PATROL_MARGIN = 5;
		private const float DEATHSTAR_LAUNCH_HOVER_MARGIN = 1.5f;
        private const float GUNSHIP_PARENT_CRUISER_MARGIN = 5;
        private const float GUNSHIP_TURN_AROUND_POINT_X = 0;
        private const float SPY_SATELLITE_PATROL_MARGIN = 5;

		public SafeZone FighterSafeZone { get; private set; }

		public AircraftProvider(Vector2 parentCruiserPosition, Vector2 enemyCruiserPosition)
		{
			_parentCruiserPosition = parentCruiserPosition;
			_enemyCruiserPosition = enemyCruiserPosition;

			float minX, maxX;

			if (parentCruiserPosition.x < 0)
			{
				minX = parentCruiserPosition.x - SAFE_ZONE_PARENT_CRUISER_OVERLAP;
				maxX = enemyCruiserPosition.x - SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE;
			}
			else
			{
				minX = enemyCruiserPosition.x + SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE;
				maxX = parentCruiserPosition.x + SAFE_ZONE_PARENT_CRUISER_OVERLAP;
			}

			FighterSafeZone = new SafeZone(
				minX: minX,
				maxX: maxX,
				minY: SAFE_ZONE_MIN_Y,
				maxY: SAFE_ZONE_MAX_Y);
		}

		public IList<Vector2> FindBomberPatrolPoints(float cruisingAltitudeInM)
		{
			float parentCruiserPatrolPointAdjustmentX = _parentCruiserPosition.x < 0 ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float parentCruiserPatrolPointX = _parentCruiserPosition.x + parentCruiserPatrolPointAdjustmentX;

			float enemyCruiserPatrolPointAdjustmentX = _enemyCruiserPosition.x < 0 ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float enemyCruiserpatrolPointX = _enemyCruiserPosition.x + enemyCruiserPatrolPointAdjustmentX;

			return new List<Vector2>() 
			{
				new Vector2(parentCruiserPatrolPointX, cruisingAltitudeInM),
				new Vector2(enemyCruiserpatrolPointX, cruisingAltitudeInM)
			};
		}

		public IList<Vector2> FindGunshipPatrolPoints(float cruisingAltitudeInM)
		{
            float parentCruiserPatrolPointAdjustmentX = _parentCruiserPosition.x < 0 ? GUNSHIP_PARENT_CRUISER_MARGIN : -GUNSHIP_PARENT_CRUISER_MARGIN;
			float parentCruiserPatrolPointX = _parentCruiserPosition.x + parentCruiserPatrolPointAdjustmentX;

			return new List<Vector2>()
			{
				new Vector2(parentCruiserPatrolPointX, cruisingAltitudeInM),
                new Vector2(GUNSHIP_TURN_AROUND_POINT_X, cruisingAltitudeInM)
			};
		}

		public IList<Vector2> FindFighterPatrolPoints(float cruisingAltitudeInM)
		{
			return new List<Vector2>() 
			{
				new Vector2(FighterSafeZone.MinX + FIGHTER_PATROL_MARGIN, cruisingAltitudeInM),
				new Vector2(FighterSafeZone.MaxX - FIGHTER_PATROL_MARGIN, cruisingAltitudeInM)
			};
		}
		
		public IList<Vector2> FindDeathstarPatrolPoints(Vector2 deathstarPosition, float cruisingAltitudeInM)
		{
			float furtherEnemyCruiserPatrolPointAdjustemntX = _enemyCruiserPosition.x > 0 ? DEATHSTAR_PATROL_MARGIN : -DEATHSTAR_PATROL_MARGIN;
			float closerEnemyCruiserPatrolPointAdjustemntX = _enemyCruiserPosition.x > 0 ? -DEATHSTAR_PATROL_MARGIN : DEATHSTAR_PATROL_MARGIN;

			return new List<Vector2>() 
			{
				new Vector2(deathstarPosition.x, deathstarPosition.y + DEATHSTAR_LAUNCH_HOVER_MARGIN),
				new Vector2(deathstarPosition.x, cruisingAltitudeInM),
				new Vector2(_enemyCruiserPosition.x + furtherEnemyCruiserPatrolPointAdjustemntX, cruisingAltitudeInM),
				new Vector2(_enemyCruiserPosition.x + closerEnemyCruiserPatrolPointAdjustemntX, cruisingAltitudeInM)
			};
		}

        public IList<Vector2> FindSpySatellitePatrolPoints(Vector2 satellitePosition, float cruisingAltitudeInM)
        {
            float closerToEnemyCruiserPatrolPointX = _enemyCruiserPosition.x > 0 ? SPY_SATELLITE_PATROL_MARGIN : -SPY_SATELLITE_PATROL_MARGIN;
            float closerToFriendlyCruiserPatrolPointX = _enemyCruiserPosition.x > 0 ? -SPY_SATELLITE_PATROL_MARGIN : SPY_SATELLITE_PATROL_MARGIN;

            return new List<Vector2>()
            {
                new Vector2(satellitePosition.x, cruisingAltitudeInM),
                new Vector2(closerToEnemyCruiserPatrolPointX, cruisingAltitudeInM),
                new Vector2(closerToFriendlyCruiserPatrolPointX, cruisingAltitudeInM)
            };
        }
	}
}
