using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Utils;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    /// <summary>
    /// Supports multiple shell spawners (barrels). The turret stats damage
    /// is spread across the barrels (i.e., if there are 2 barrels, each barrel
    /// receives half the turret stats' damage).
    /// </summary>
    public class ShellTurretBarrelController : BarrelController
    {
        private ShellSpawner[] _shellSpawners;
        private ShellSpawner _middleSpawner;

        protected override int NumOfBarrels => _shellSpawners.Length;

        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => true;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _shellSpawners = gameObject.GetComponentsInChildren<ShellSpawner>();
            Assert.IsNotNull(_shellSpawners);
            Assert.IsTrue(_shellSpawners.Length != 0);

            _middleSpawner = _shellSpawners.Middle();
        }

        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
        {
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args, _projectileStats, TurretStats.BurstSize);

            foreach (ShellSpawner spawner in _shellSpawners)
            {
                await spawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey, args.TargetFilter, TurretStats.AttackCapabilities.ToList());
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
