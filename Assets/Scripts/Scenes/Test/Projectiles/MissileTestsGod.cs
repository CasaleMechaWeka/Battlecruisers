using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
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

            ProjectileStats stats = GetComponent<ProjectileStats>();
            IProjectileStats missileStats = new ProjectileStatsWrapper(stats);

			Vector2 initialVelocity = new Vector2(5, 5);
			IMovementControllerFactory movementControllerFactory = new MovementControllerFactory(null, null);
			ITargetPositionPredictorFactory targetPositionPredictorFactory = new TargetPositionPredictorFactory();

            MissileController[] missiles = FindObjectsOfType<MissileController>();
			foreach (MissileController missile in missiles)
			{
				missile.Initialise(missileStats, initialVelocity, targetFilter, target, movementControllerFactory, targetPositionPredictorFactory);
			}
		}
	}
}
