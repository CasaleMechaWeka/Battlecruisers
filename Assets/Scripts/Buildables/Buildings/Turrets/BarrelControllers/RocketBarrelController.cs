using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	public class RocketBarrelController : TurretBarrelController
	{
		private RocketSpawner _rocketSpawner;

		public RocketController rocketPrefab;

		// FELIX  Use!
		private const float ROCKET_LAUNCH_ANGLE_IN_DEGREES = 90;
		private const float ROCKET_CRUISING_ALTITUDE_IN_M = 25;

		public void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, IMovementControllerFactory movementControllerFactory)
		{
			base.Initialise(targetFilter, angleCalculator);

			Assert.IsNotNull(rocketPrefab);

			_rocketSpawner = gameObject.GetComponentInChildren<RocketSpawner>();
			Assert.IsNotNull(_rocketSpawner);

			RocketStats rocketStats = new RocketStats(rocketPrefab, TurretStats.damage, TurretStats.bulletVelocityInMPerS, ROCKET_CRUISING_ALTITUDE_IN_M);
			_rocketSpawner.Initialise(rocketStats, movementControllerFactory);
		}

		protected override void Fire(float angleInDegrees)
		{
			_rocketSpawner.SpawnRocket(
				angleInDegrees,
				transform.IsMirrored(),
				Target,
				_targetFilter,
				Faction);
		}
	}
}

