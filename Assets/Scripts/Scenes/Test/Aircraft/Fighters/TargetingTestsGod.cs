using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
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
    public class TargetingTestsGod : MonoBehaviour 
	{
		private Helper _helper;

		public List<Vector2> fighterPatrolPoints, targetPatrolPoints;

		void Start() 
		{
			_helper = new Helper();

			FighterController fighter = FindObjectOfType<FighterController>();
			IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(fighterPatrolPoints: fighterPatrolPoints);
            _helper.InitialiseUnit(fighter, Faction.Reds, aircraftProvider: aircraftProvider);
			fighter.StartConstruction();

			TestAircraftController target = FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
            _helper.InitialiseUnit(target, faction: Faction.Blues);
			target.StartConstruction();
		}
	}
}
