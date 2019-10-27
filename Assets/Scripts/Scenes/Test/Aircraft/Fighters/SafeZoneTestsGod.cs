using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    /// <summary>
    /// 1. Fighter is patrolling
    /// 2. Target is patrolling
    /// 3. Target enters fighter's "safe" zone
    /// 4. Fighter "sees" target, start attacking
    /// 5. Target moves out of fighter's "safe" zone
    /// 6. Fighter abandons chase
    /// 7. Repeat
    /// </summary>
    public class SafeZoneTestsGod : TestGodBase
	{
		private FighterController _fighter;
        private TestAircraftController _target;

        public List<Vector2> fighterPatrolPoints, targetPatrolPoints;
		public float safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY;

        protected override List<GameObject> GetGameObjects()
        {
            _fighter = FindObjectOfType<FighterController>();
            _target = FindObjectOfType<TestAircraftController>();

            return new List<GameObject>()
            {
                _fighter.GameObject,
                _target.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

			Rectangle safeZone = new Rectangle(safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints, fighterSafeZone: safeZone);
            helper.InitialiseUnit(_fighter, Faction.Reds, aircraftProvider: aircraftProvider, enemyCruiser: blueCruiser);
			_fighter.StartConstruction();

			// Target aircraft
			_target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(_target, faction: Faction.Blues);
			_target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(_target, blueCruiser);
		}
	}
}
