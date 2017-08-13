using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Scenes.Test.Utilities;
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
			TestAircraftController target = FindObjectOfType<TestAircraftController>();
			target.PatrolPoints = targetPatrolPoints;
			target.SetTargetType(TargetType.Ships);  // So mortars will attack this
			helper.InitialiseBuildable(target, Faction.Blues);
			target.StartConstruction();

            // Setup mortars
            MortarController[] mortars = FindObjectsOfType<MortarController>();
			foreach (MortarController mortar in mortars)
			{
				helper.InitialiseBuildable(mortar, Faction.Reds);
				mortar.StartConstruction();
			}
		}
	}
}
