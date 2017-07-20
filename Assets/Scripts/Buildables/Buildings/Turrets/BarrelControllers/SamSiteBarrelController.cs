using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class SamSiteBarrelController : BarrelController
	{
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private MissileSpawner _missileSpawner;

		public MissileController missilePrefab;

		public void Initialise(IExactMatchTargetFilter targetFilter, IAngleCalculator angleCalculator, IRotationMovementController rotationMovementController,
			IMovementControllerFactory movementControllerFactory, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			base.Initialise(targetFilter, angleCalculator, rotationMovementController);

			Assert.IsNotNull(missilePrefab);

			_exactMatchTargetFilter = targetFilter;

			_missileSpawner = gameObject.GetComponentInChildren<MissileSpawner>();
			Assert.IsNotNull(_missileSpawner);

			MissileStats missileStats = new MissileStats(missilePrefab, TurretStats.damage, TurretStats.bulletVelocityInMPerS);
			_missileSpawner.Initialise(missileStats, movementControllerFactory, targetPositionPredictorFactory);
		}

		protected override void Fire(float angleInDegrees)
		{
			_exactMatchTargetFilter.Target = Target;
			_missileSpawner.SpawnMissile(angleInDegrees, IsSourceMirrored, Target, _exactMatchTargetFilter);
		}
	}
}
