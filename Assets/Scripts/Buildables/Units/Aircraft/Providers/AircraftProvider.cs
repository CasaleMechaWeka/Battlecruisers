using System.Collections.Generic;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.Providers
{
    public class AircraftProvider : IAircraftProvider
	{
		private readonly Vector2 _parentCruiserPosition, _enemyCruiserPosition;
        private readonly IRandomGenerator _random;

		private const float SAFE_ZONE_PARENT_CRUISER_OVERLAP = 10;
		private const float SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE = 25;
		private const float SAFE_ZONE_MIN_Y = 10;
		private const float SAFE_ZONE_MAX_Y = 25;

		private const float FIGHTER_PATROL_MARGIN = 5;
		private const float BOMBER_PATROL_MARGIN = 10;
		private const float DEATHSTAR_PATROL_MARGIN = 5;
		private const float DEATHSTAR_LAUNCH_HOVER_MARGIN = 1.5f;
        private const float GUNSHIP_PARENT_CRUISER_MARGIN = 5;
        private const float SPY_SATELLITE_PATROL_MARGIN = 5;
        public const float CRUISING_ALTITUDE_ERROR_MARGIN_IN_M = 1;

        private bool IsEnemyToTheRight { get { return _enemyCruiserPosition.x > _parentCruiserPosition.x; } }
		public Rectangle FighterSafeZone { get; private set; }

		public AircraftProvider(Vector2 parentCruiserPosition, Vector2 enemyCruiserPosition, IRandomGenerator random)
		{
			_parentCruiserPosition = parentCruiserPosition;
			_enemyCruiserPosition = enemyCruiserPosition;

            Assert.IsNotNull(random);
            _random = random;

			float minX, maxX;

            if (IsEnemyToTheRight)
			{
                // Enemy is to the right
				minX = parentCruiserPosition.x - SAFE_ZONE_PARENT_CRUISER_OVERLAP;
				maxX = enemyCruiserPosition.x - SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE;
			}
			else
			{
                // Enemy is to the left
				minX = enemyCruiserPosition.x + SAFE_ZONE_ENEMY_CRUISER_AVOIDANCE;
				maxX = parentCruiserPosition.x + SAFE_ZONE_PARENT_CRUISER_OVERLAP;
			}

			FighterSafeZone = new Rectangle(
				minX: minX,
				maxX: maxX,
				minY: SAFE_ZONE_MIN_Y,
				maxY: SAFE_ZONE_MAX_Y);
		}

		public IList<Vector2> FindBomberPatrolPoints(float cruisingAltitudeInM)
		{
            // Only let bombers fuzz downwards, so:
            // 1. They will always be in range of AA guns
            // 2. The don't cover the tutorial explanation text :)
            cruisingAltitudeInM = FuzzCruisingAltitude(cruisingAltitudeInM, onlyDownwards: true);

            float parentCruiserPatrolPointAdjustmentX = IsEnemyToTheRight ? BOMBER_PATROL_MARGIN : -BOMBER_PATROL_MARGIN;
			float parentCruiserPatrolPointX = _parentCruiserPosition.x + parentCruiserPatrolPointAdjustmentX;

            float enemyCruiserPatrolPointAdjustmentX = IsEnemyToTheRight ? -BOMBER_PATROL_MARGIN : BOMBER_PATROL_MARGIN;
			float enemyCruiserpatrolPointX = _enemyCruiserPosition.x + enemyCruiserPatrolPointAdjustmentX;

			return new List<Vector2>() 
			{
				new Vector2(parentCruiserPatrolPointX, cruisingAltitudeInM),
				new Vector2(enemyCruiserpatrolPointX, cruisingAltitudeInM)
			};
		}

		public IList<Vector2> FindGunshipPatrolPoints(float cruisingAltitudeInM)
		{
            cruisingAltitudeInM = FuzzCruisingAltitude(cruisingAltitudeInM);

            float parentCruiserPatrolPointAdjustmentX = IsEnemyToTheRight ? GUNSHIP_PARENT_CRUISER_MARGIN : -GUNSHIP_PARENT_CRUISER_MARGIN;
			float parentCruiserPatrolPointX = _parentCruiserPosition.x + parentCruiserPatrolPointAdjustmentX;
            float gunshipTurnAroundPointX = (_parentCruiserPosition.x + _enemyCruiserPosition.x) / 2;

			return new List<Vector2>()
			{
				new Vector2(parentCruiserPatrolPointX, cruisingAltitudeInM),
                new Vector2(gunshipTurnAroundPointX, cruisingAltitudeInM)
			};
		}

		public IList<Vector2> FindFighterPatrolPoints(float cruisingAltitudeInM)
		{
            cruisingAltitudeInM = FuzzCruisingAltitude(cruisingAltitudeInM);

            float parentCruiserPatrolPoint = IsEnemyToTheRight ? FighterSafeZone.MinX + FIGHTER_PATROL_MARGIN : FighterSafeZone.MaxX - FIGHTER_PATROL_MARGIN;
            float middlePatrolPoint = IsEnemyToTheRight ? FighterSafeZone.MaxX - FIGHTER_PATROL_MARGIN : FighterSafeZone.MinX + FIGHTER_PATROL_MARGIN;

			return new List<Vector2>() 
			{
                new Vector2(parentCruiserPatrolPoint, cruisingAltitudeInM),
                new Vector2(middlePatrolPoint, cruisingAltitudeInM)
			};
		}
		
		public IList<Vector2> FindDeathstarPatrolPoints(Vector2 deathstarPosition, float cruisingAltitudeInM)
		{
            float furtherEnemyCruiserPatrolPointAdjustemntX = IsEnemyToTheRight ? DEATHSTAR_PATROL_MARGIN : -DEATHSTAR_PATROL_MARGIN;
            float closerEnemyCruiserPatrolPointAdjustemntX = IsEnemyToTheRight ? -DEATHSTAR_PATROL_MARGIN : DEATHSTAR_PATROL_MARGIN;

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
            float closerToEnemyCruiserPatrolPointX = IsEnemyToTheRight ? SPY_SATELLITE_PATROL_MARGIN : -SPY_SATELLITE_PATROL_MARGIN;
            float closerToFriendlyCruiserPatrolPointX = IsEnemyToTheRight ? -SPY_SATELLITE_PATROL_MARGIN : SPY_SATELLITE_PATROL_MARGIN;

            return new List<Vector2>()
            {
                new Vector2(satellitePosition.x, cruisingAltitudeInM),
                new Vector2(closerToEnemyCruiserPatrolPointX, cruisingAltitudeInM),
                new Vector2(closerToFriendlyCruiserPatrolPointX, cruisingAltitudeInM)
            };
        }

        /// <summary>
		/// Randomise cruising altitude slightly, to avoid all planes
		/// flying at exactly the same height :P
		/// </summary>
        private float FuzzCruisingAltitude(float cruisingAltitudeInM, bool onlyDownwards = false)
        {
            if (onlyDownwards)
            {
                cruisingAltitudeInM = cruisingAltitudeInM - CRUISING_ALTITUDE_ERROR_MARGIN_IN_M;
            }

            return _random.RangeFromCenter(cruisingAltitudeInM, CRUISING_ALTITUDE_ERROR_MARGIN_IN_M); 
        }
	}
}
