using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Utils;
using UnityEngine;
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

        protected override int NumOfBarrels => _shellSpawners.Length;

        // PERF  Cache middle spawner, so don't need to run method each time
        public override Vector3 ProjectileSpawnerPosition => _shellSpawners.Middle().transform.position;
        public override bool CanFireWithoutTarget => true;

        public override void StaticInitialise()
		{
			base.StaticInitialise();

			_shellSpawners = gameObject.GetComponentsInChildren<ShellSpawner>();
			Assert.IsNotNull(_shellSpawners);
            Assert.IsTrue(_shellSpawners.Length != 0);
		}

        public override void Initialise(IBarrelControllerArgs args)
		{
            base.Initialise(args);

            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args.Parent, _projectileStats, TurretStats.BurstSize, args.FactoryProvider);

            foreach (ShellSpawner spawner in _shellSpawners)
            {
                spawner.Initialise(spawnerArgs, args.TargetFilter, args.SpawnerSoundKey);
            }
		}

        public override void Fire(float angleInDegrees)
		{
			foreach (ShellSpawner spawner in _shellSpawners)
            {
                spawner.SpawnShell(angleInDegrees, IsSourceMirrored);
			}
		}
	}
}

