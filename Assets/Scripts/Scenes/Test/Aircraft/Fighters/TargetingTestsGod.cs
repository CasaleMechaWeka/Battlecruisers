using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
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
		private FighterController _fighter;
        private TestAircraftController _target;

        public List<Vector2> fighterPatrolPoints, targetPatrolPoints;

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

			IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints);
            helper.InitialiseUnit(_fighter, Faction.Reds, aircraftProvider: aircraftProvider, enemyCruiser: blueCruiser);
			_fighter.StartConstruction();

			_target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(_target, faction: Faction.Blues);
			_target.StartConstruction();
            Helper.SetupUnitForUnitMonitor(_target, blueCruiser);
		}
	}
}
