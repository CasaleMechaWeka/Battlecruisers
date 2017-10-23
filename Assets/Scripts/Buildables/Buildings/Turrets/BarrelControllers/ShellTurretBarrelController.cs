using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    /// <summary>
    /// Supports mutliple shell spawners (barrels).  The turret stats damage
    /// is spread accross the barrels (ie, if there are 2 barrels, each barrel
    /// receives half the turret stats' damage).
    /// </summary>
    public class ShellTurretBarrelController : BarrelController
	{
        private ShellSpawner[] _shellSpawners;
        private float _damagePerBarrel;

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_shellSpawners = gameObject.GetComponentsInChildren<ShellSpawner>();
			Assert.IsNotNull(_shellSpawners);
            Assert.IsTrue(_shellSpawners.Length != 0);

            _damagePerBarrel = _projectileStats.Damage / _shellSpawners.Length;
		}

		public override void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, IRotationMovementController rotationMovementController)
		{
			base.Initialise(targetFilter, angleCalculator, rotationMovementController);

            foreach (ShellSpawner spawner in _shellSpawners)
            {
                spawner.Initialise(_projectileStats, targetFilter);
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

