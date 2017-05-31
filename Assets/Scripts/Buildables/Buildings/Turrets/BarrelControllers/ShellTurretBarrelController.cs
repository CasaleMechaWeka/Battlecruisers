using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	public class ShellTurretBarrelController : TurretBarrelController
	{
		private ShellSpawnerController _shellSpawner;

		public ShellController shellPrefab;

		public override void Initialise(Faction faction, IAngleCalculator angleCalculator, IMovementControllerFactory movementControllerFactory, 
			ITargetPositionPredictorFactory targetPositionPredictorFactory, ITargetsFactory targetsFactory)
		{
			base.Initialise(faction, angleCalculator, movementControllerFactory, targetPositionPredictorFactory, targetsFactory);

			Assert.IsNotNull(shellPrefab);

			_shellSpawner = gameObject.GetComponentInChildren<ShellSpawnerController>();
			Assert.IsNotNull(_shellSpawner);

			ShellStats _shellStats = new ShellStats(shellPrefab, turretStats.damage, turretStats.ignoreGravity, turretStats.bulletVelocityInMPerS);
			_shellSpawner.Initialise(_faction, _shellStats);
		}

		protected override void Fire(float angleInDegrees)
		{
			_shellSpawner.SpawnShell(angleInDegrees, IsSourceMirrored);
		}
	}
}

