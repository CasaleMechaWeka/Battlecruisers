using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileMovingTargetTestsGod : MissileTestsGod 
	{
		public AircraftController target;
		public List<Vector2> targetPatrolPoints;

		void Start () 
		{
			Helper helper = new Helper();

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
