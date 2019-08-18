using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.DataStrctures;
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
		private Helper helper;

		public List<Vector2> fighterPatrolPoints, targetPatrolPoints;
		public float safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY;

        protected override void Start()
        {
            base.Start();

            helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            FighterController fighter = FindObjectOfType<FighterController>();
			Rectangle safeZone = new Rectangle(safeZoneMinX, safeZoneMaxX, safeZoneMinY, safeZoneMaxY);
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints, fighterSafeZone: safeZone);
            helper.InitialiseUnit(fighter, Faction.Reds, aircraftProvider: aircraftProvider, enemyCruiser: blueCruiser);
			fighter.StartConstruction();

			// Target aircraft
			TestAircraftController target = FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(target, faction: Faction.Blues);
			target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(target, blueCruiser);
		}
	}
}
