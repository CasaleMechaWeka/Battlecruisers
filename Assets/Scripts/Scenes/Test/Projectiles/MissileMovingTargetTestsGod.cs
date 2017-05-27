using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Units.Aircraft;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileMovingTargetTestsGod : MonoBehaviour 
	{
		public AircraftController target;
		public List<Vector2> targetPatrolPoints;

		void Start () 
		{
			Helper helper = new Helper();


			// Setup target
			helper.InitialiseBuildable(target);
			target.CompletedBuildable += (sender, e) => 
			{
				target.PatrolPoints = targetPatrolPoints;
				target.StartPatrolling();
			};
			target.StartConstruction();


			// Setup missiles
			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			MissileStats missileStats = new MissileStats(damage: 50, maxVelocityInMPerS: 20);
			Vector2 initialVelocity = new Vector2(5, 5);

			MissileController[] missiles = GameObject.FindObjectsOfType<MissileController>() as MissileController[];
			foreach (MissileController missile in missiles)
			{
				missile.Initialise(target, targetFilter, missileStats, initialVelocity);
			}
		}
	}
}
