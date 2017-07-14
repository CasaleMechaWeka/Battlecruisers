using BattleCruisers.Movement.Velocity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
		private const float DEATHSTAR_LAUNCH_HOVER_MARGIN = 1;

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

		public IList<IPatrolPoint> FindBomberPatrolPoints(float cruisingAltitudeInM, Action onFirstPatrolPointReached)
		{
			float parentCruiserPatrolPointAdjustmentX = _parentCruiserPosition.x < 0 ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float parentCruiserPatrolPointX = _parentCruiserPosition.x + parentCruiserPatrolPointAdjustmentX;

			float enemyCruiserPatrolPointAdjustmentX = _enemyCruiserPosition.x < 0 ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float enemyCruiserpatrolPointX = _enemyCruiserPosition.x + enemyCruiserPatrolPointAdjustmentX;

			return new List<IPatrolPoint>() 
			{
				new PatrolPoint(new Vector2(parentCruiserPatrolPointX, cruisingAltitudeInM), removeOnceReached: false, actionOnReached: onFirstPatrolPointReached),
				new PatrolPoint(new Vector2(enemyCruiserpatrolPointX, cruisingAltitudeInM))
			};
		}

		public IList<IPatrolPoint> FindFighterPatrolPoints(float cruisingAltitudeInM)
		{
			return new List<IPatrolPoint>() 
			{
				new PatrolPoint(new Vector2(FighterSafeZone.MinX + FIGHTER_PATROL_MARGIN, cruisingAltitudeInM)),
				new PatrolPoint(new Vector2(FighterSafeZone.MaxX - FIGHTER_PATROL_MARGIN, cruisingAltitudeInM))
			};
		}
		
		public IList<IPatrolPoint> FindDeathstarPatrolPoints(Vector2 deathstarPosition, float cruisingAltitudeInM, Action onFirstPatrolPointReached)
		{
			return new List<IPatrolPoint>() {
				new PatrolPoint(new Vector2(deathstarPosition.x, deathstarPosition.y + DEATHSTAR_LAUNCH_HOVER_MARGIN), removeOnceReached: true, actionOnReached: onFirstPatrolPointReached),
				new PatrolPoint(new Vector2(deathstarPosition.x, cruisingAltitudeInM), removeOnceReached: true),
				new PatrolPoint(new Vector2(_enemyCruiserPosition.x + DEATHSTAR_PATROL_MARGIN, cruisingAltitudeInM)),
				new PatrolPoint(new Vector2(_enemyCruiserPosition.x - DEATHSTAR_PATROL_MARGIN, cruisingAltitudeInM))
			};
		}
	}
}
