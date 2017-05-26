using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FELIX  Identical script as Stationary target script?  Just use one instead of dupplicating?
namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
	public class MortarMovingTargetTestGod : MonoBehaviour 
	{
		private Helper _helper;

		public AircraftController target;
		public List<Vector2> targetPatrolPoints;
		public DefensiveTurret mortarLeftLow, mortarLeftMiddle, mortarLeftHigh;
		public DefensiveTurret mortarRightLow, mortarRightMiddle, mortarRightHigh;

		void Start () 
		{
			_helper = new Helper();

			target.PatrolPoints = targetPatrolPoints;
			target.CompletedBuildable += (sender, e) => target.StartPatrolling();
			_helper.InitialiseBuildable(target, Faction.Blues);
			target.StartConstruction();

//			SetupPair(mortarLeftLow, target);
			SetupPair(mortarLeftMiddle, target.GameObject);
//			SetupPair(mortarLeftHigh, target);
//
//			SetupPair(mortarRightLow, target);
//			SetupPair(mortarRightMiddle, target);
//			SetupPair(mortarRightHigh, target);
		}

		private void SetupPair(DefensiveTurret mortar, GameObject target)
		{
			_helper.InitialiseBuildable(mortar, Faction.Reds);
			mortar.StartConstruction();
		}
	}
}
