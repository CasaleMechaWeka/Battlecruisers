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
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private MissileSpawner _missileSpawner;
		private ITargetsFactory _targetsFactory;

		public MissileController missilePrefab;

		public void Initialise(IExactMatchTargetFilter targetFilter, IAngleCalculator angleCalculator, 
			IMovementControllerFactory movementControllerFactory, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			base.Initialise(targetFilter, angleCalculator);

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

