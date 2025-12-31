using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class FirecrackerMissileBarrelController : BarrelController
    {
        private ICircularList<FirecrackerMissileSpawner> _missileSpawners;
        private FirecrackerMissileSpawner _middleSpawner;
        private ProjectileStats _missileStats;

        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            FirecrackerMissileSpawner[] missileSpawners = gameObject.GetComponentsInChildren<FirecrackerMissileSpawner>();
            Assert.IsTrue(missileSpawners.Length != 0);
            _missileSpawners = new CircularList<FirecrackerMissileSpawner>(missileSpawners);

            _middleSpawner = missileSpawners.Middle();
        }

        protected override ProjectileStats GetProjectileStats()
        {
            _missileStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(_missileStats);
            return _missileStats;
        }

        protected override async Task InternalInitialiseAsync(BarrelControllerArgs args)
        {
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args, _missileStats, TurretStats.BurstSize);

            foreach (FirecrackerMissileSpawner missileSpawner in _missileSpawners.Items)
            {
                await missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.FiringSoundToKey(FiringSound), _missileStats);
            }
        }

        public override void Fire(float angleInDegrees)
        {
            _missileSpawners
                .Next()
                .SpawnRocket(
                    angleInDegrees,
                    transform.IsMirrored(),
                    Target,
                    _targetFilter);
        }
    }
}
