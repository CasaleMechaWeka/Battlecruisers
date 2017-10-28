using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    // FELIX  Use BarrelController instead where possible (to reference ShellTurretBarrelController)

    /// <summary>
    /// Supports mutliple shell spawners (barrels).  The turret stats damage
    /// is spread accross the barrels (ie, if there are 2 barrels, each barrel
    /// receives half the turret stats' damage).
    /// </summary>
    public class ShellTurretBarrelController : BarrelController
	{
        private ShellSpawner[] _shellSpawners;

        protected override int NumOfBarrels { get { return _shellSpawners.Length; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_shellSpawners = gameObject.GetComponentsInChildren<ShellSpawner>();
			Assert.IsNotNull(_shellSpawners);
            Assert.IsTrue(_shellSpawners.Length != 0);
		}

		public override void Initialise(
            ITargetFilter targetFilter,
            IAngleCalculator angleCalculator,
            IRotationMovementController rotationMovementController,
            IFactoryProvider factoryProvider)
		{
            base.Initialise(targetFilter, angleCalculator, rotationMovementController, factoryProvider);

            foreach (ShellSpawner spawner in _shellSpawners)
            {
                spawner.Initialise(_projectileStats, targetFilter, factoryProvider.DamageApplierFactory);
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

