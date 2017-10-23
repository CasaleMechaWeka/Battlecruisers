using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class MissileSpawner : ProjectileSpawner
	{
		private IMovementControllerFactory _movementControllerFactory;
		private ITargetPositionPredictorFactory _targetPositionPredictorFactory;

        public MissileController missilePrefab;
        protected override ProjectileController ProjectilePrefab { get { return missilePrefab; } }

        public void Initialise(IProjectileStats missileStats, IMovementControllerFactory movementControllerFactory, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
            base.Initialise(missileStats);

            Helper.AssertIsNotNull(movementControllerFactory, targetPositionPredictorFactory);

			_movementControllerFactory = movementControllerFactory;
			_targetPositionPredictorFactory = targetPositionPredictorFactory;
		}

		public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
            MissileController missile = Instantiate(missilePrefab, transform.position, new Quaternion());
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            missile.Initialise(_projectileStats, missileVelocity, targetFilter, target, _movementControllerFactory, _targetPositionPredictorFactory);
		}
	}
}
