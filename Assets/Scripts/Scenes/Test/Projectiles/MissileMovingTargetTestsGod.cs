using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileMovingTargetTestsGod : MissileTestsGod 
	{
		public List<Vector2> targetPatrolPoints;

		void Start () 
		{
			Helper helper = new Helper();

			AircraftController target = GameObject.FindObjectOfType<AircraftController>();
			helper.InitialiseBuildable(target);
			target.CompletedBuildable += (sender, e) => 
			{
				target.PatrolPoints = targetPatrolPoints;
				target.StartPatrolling();
			};
			target.StartConstruction();

			SetupMissiles(target);
		}
	}
}
