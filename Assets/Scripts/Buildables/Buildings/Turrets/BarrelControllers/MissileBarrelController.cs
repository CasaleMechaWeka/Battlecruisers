using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class MissileBarrelController : BarrelController
    {
        private ICircularList<MissileSpawner> _missileSpawners;
        private MissileSpawner _middleSpawner;
        public float delayInS;
        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            MissileSpawner[] missileSpawners = gameObject.GetComponentsInChildren<MissileSpawner>();
            Assert.IsTrue(missileSpawners.Length != 0);
            _missileSpawners = new CircularList<MissileSpawner>(missileSpawners);

            _middleSpawner = missileSpawners.Middle();
        }

        protected override async Task InternalInitialiseAsync(BarrelControllerArgs args)
        {
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args, _projectileStats, TurretStats.BurstSize);

            foreach (MissileSpawner missileSpawner in _missileSpawners.Items)
            {
                await missileSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey);
            }
        }

        public override async void Fire(float angleInDegrees)
        {
            Logging.Log(Tags.BARREL_CONTROLLER, $"{this}  angleInDegrees: " + angleInDegrees);
            if (Target == null)
            {
                Logging.Log(Tags.BARREL_CONTROLLER, $"Target was null");

            }
            else
            {
                await Task.Delay((int)(delayInS * 1000f));
                _missileSpawners.Next().SpawnMissile(
                    angleInDegrees,
                    IsSourceMirrored,
                    Target,
                    _targetFilter);
            }
        }
    }
}
