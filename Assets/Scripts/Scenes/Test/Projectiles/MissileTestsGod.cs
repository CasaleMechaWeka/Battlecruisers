using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
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

            // FELIX  Create in inspector
            IProjectileStats missileStats
                = new ProjectileStatsWrapper(
                    damage: 50,
                    maxVelocityInMPerS: 20,
                    ignoreGravity: true,
                    hasAreaOfEffectDamage: true,
                    damageRadiusInM: 2,
                    initialVelocityMultiplier: 0.25f);
            
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
