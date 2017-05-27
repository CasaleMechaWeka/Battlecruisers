using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
	public class MortarMovingTargetTestGod : MonoBehaviour 
	{
		private Helper _helper;

		public AircraftController target;
		public List<Vector2> targetPatrolPoints;

		void Start () 
		{
			_helper = new Helper();

			// Setup target
			target.PatrolPoints = targetPatrolPoints;
			target.CompletedBuildable += (sender, e) => target.StartPatrolling();
			_helper.InitialiseBuildable(target, Faction.Blues);
			target.StartConstruction();

			// Setup mortars
			DefensiveTurret[] mortars = GameObject.FindObjectsOfType(typeof(DefensiveTurret)) as DefensiveTurret[];
			foreach (DefensiveTurret mortar in mortars)
			{
				_helper.InitialiseBuildable(mortar, Faction.Reds);
				mortar.StartConstruction();
			}
		}
	}
}
