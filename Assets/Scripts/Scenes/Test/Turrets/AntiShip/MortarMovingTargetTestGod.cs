using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Buildables.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
	public class MortarMovingTargetTestGod : MonoBehaviour 
	{
		public List<Vector2> targetPatrolPoints;

		void Start () 
		{
			Helper helper = new Helper();

			// Setup target
			TestAircraftController target = GameObject.FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
			target.SetTargetType(TargetType.Ships);  // So mortars will attack this
			helper.InitialiseBuildable(target, Faction.Blues);
			target.StartConstruction();

			// Setup mortars
			MortarController[] mortars = GameObject.FindObjectsOfType(typeof(MortarController)) as MortarController[];
			foreach (MortarController mortar in mortars)
			{
				helper.InitialiseBuildable(mortar, Faction.Reds);
				mortar.StartConstruction();
			}
		}
	}
}
