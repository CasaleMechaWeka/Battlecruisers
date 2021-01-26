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

        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
        {
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args.Parent, _projectileStats, TurretStats.BurstSize, args.FactoryProvider);

            foreach (MissileSpawner missileSpawner in _missileSpawners.Items)
            {
                await missileSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey);
			}
		}

        public override void Fire(float angleInDegrees)
		{
            Logging.Log(Tags.BARREL_CONTROLLER, "angleInDegrees: " + angleInDegrees);

            _missileSpawners.Next().SpawnMissile(
                angleInDegrees,
                IsSourceMirrored,
                Target,
                _targetFilter);
		}
	}
}
