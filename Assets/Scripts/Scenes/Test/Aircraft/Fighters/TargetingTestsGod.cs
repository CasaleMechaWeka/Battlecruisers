using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    /// <summary>
    /// 1. Fighter is patrolling
    /// 2. Target is patrolling very quickly
    /// 3. Fighter "sees" targets, start attacking
    /// 4. Target quickly moves out of range, fighter stops pursuing target and continues patrolling
    /// 5. Repeat
    /// </summary>
    public class TargetingTestsGod : TestGodBase 
	{
		private Helper helper;

		public List<Vector2> fighterPatrolPoints, targetPatrolPoints;

        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            FighterController fighter = FindObjectOfType<FighterController>();
			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints);
            helper.InitialiseUnit(fighter, Faction.Reds, aircraftProvider: aircraftProvider, enemyCruiser: blueCruiser);
			fighter.StartConstruction();

			TestAircraftController target = FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(target, faction: Faction.Blues);
			target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(target, blueCruiser);
		}
	}
}
