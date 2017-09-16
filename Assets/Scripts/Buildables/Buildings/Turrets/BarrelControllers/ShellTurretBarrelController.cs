using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class ShellTurretBarrelController : BarrelController
	{
        private ShellSpawner[] _shellSpawners;

		public ProjectileController shellPrefab;

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(shellPrefab);
			
			_shellSpawners = gameObject.GetComponentsInChildren<ShellSpawner>();
			Assert.IsNotNull(_shellSpawners);
            Assert.IsTrue(_shellSpawners.Length != 0);
		}

		public override void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, IRotationMovementController rotationMovementController)
		{
			base.Initialise(targetFilter, angleCalculator, rotationMovementController);

			ShellStats shellStats = new ShellStats(shellPrefab, TurretStats.damage, TurretStats.ignoreGravity, TurretStats.bulletVelocityInMPerS);

            foreach (ShellSpawner spawner in _shellSpawners)
            {
                spawner.Initialise(shellStats, targetFilter);
            }
		}

		protected override void Fire(float angleInDegrees)
		{
			foreach (ShellSpawner spawner in _shellSpawners)
            {
                spawner.SpawnShell(angleInDegrees, IsSourceMirrored);
			}
		}
	}
}

