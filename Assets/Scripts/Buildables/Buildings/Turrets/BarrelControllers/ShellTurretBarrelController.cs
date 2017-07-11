using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Velocity;
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
	public class ShellTurretBarrelController : TurretBarrelController
	{
		private ShellSpawner _shellSpawner;

		public ProjectileController shellPrefab;

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(shellPrefab);
			
			_shellSpawner = gameObject.GetComponentInChildren<ShellSpawner>();
			Assert.IsNotNull(_shellSpawner);
		}

		public override void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator)
		{
			base.Initialise(targetFilter, angleCalculator);

			ShellStats shellStats = new ShellStats(shellPrefab, TurretStats.damage, TurretStats.ignoreGravity, TurretStats.bulletVelocityInMPerS);
			_shellSpawner.Initialise(shellStats, targetFilter);
		}

		protected override void Fire(float angleInDegrees)
		{
			_shellSpawner.SpawnShell(angleInDegrees, IsSourceMirrored);
		}
	}
}

