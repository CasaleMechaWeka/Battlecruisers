using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class MissileTestsGod : MonoBehaviour 
	{
		protected void SetupMissiles(ITarget target)
		{
			// Setup missiles
			IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter() 
			{
				Target = target
			};
			MissileStats missileStats = new MissileStats(damage: 50, maxVelocityInMPerS: 20);
			Vector2 initialVelocity = new Vector2(5, 5);
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory();
			ITargetPositionPredictorFactory targetPositionPredictorFactory = new TargetPositionPredictorFactory();

			MissileController[] missiles = GameObject.FindObjectsOfType<MissileController>() as MissileController[];
			foreach (MissileController missile in missiles)
			{
				missile.Initialise(target, targetFilter, missileStats, initialVelocity, movementControllerFactory, targetPositionPredictorFactory);
			}
		}
	}
}
