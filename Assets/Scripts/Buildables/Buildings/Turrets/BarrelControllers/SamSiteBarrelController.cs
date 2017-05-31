using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	public class SamSiteBarrelController : TurretBarrelController
	{
		private MissileSpawnerController _missileSpawner;
		private IMovementControllerFactory _movementControllerFactory;
		private ITargetPositionPredictorFactory _targetPositionPredictorFactory;
		private ITargetsFactory _targetsFactory;

		public MissileController missilePrefab;

		public void Initialise(Faction faction, IAngleCalculator angleCalculator, IMovementControllerFactory movementControllerFactory, 
			ITargetPositionPredictorFactory targetPositionPredictorFactory, ITargetsFactory targetsFactory)
		{
			base.Initialise(faction, angleCalculator);

			Assert.IsNotNull(missilePrefab);

			_missileSpawner = gameObject.GetComponentInChildren<MissileSpawnerController>();
			Assert.IsNotNull(_missileSpawner);

			MissileStats missileStats = new MissileStats(missilePrefab, turretStats.damage, turretStats.bulletVelocityInMPerS);
			_missileSpawner.Initialise(missileStats, _movementControllerFactory, _targetPositionPredictorFactory);
		}

		protected override void Fire(float angleInDegrees)
		{
			IExactMatchTargetFilter targetFilter = _targetsFactory.CreateExactMatchTargetFiler();
			targetFilter.Target = Target;
			_missileSpawner.SpawnMissile(angleInDegrees, IsSourceMirrored, Target, targetFilter);
		}
	}
}

