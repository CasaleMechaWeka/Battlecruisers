using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class RocketBarrelController : BarrelController
	{
		private ICircularList<RocketSpawner> _rocketSpawners;
        private RocketSpawner _middleSpawner;
        private ICruisingProjectileStats _rocketStats;

        public override Vector3 ProjectileSpawnerPosition => _middleSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            RocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<RocketSpawner>();
            Assert.IsTrue(rocketSpawners.Length != 0);
            _rocketSpawners = new CircularList<RocketSpawner>(rocketSpawners);

            _middleSpawner = rocketSpawners.Middle();
        }

        protected override IProjectileStats GetProjectileStats()
        {
            _rocketStats = GetComponent<CruisingProjectileStats>();
			Assert.IsNotNull(_rocketStats);
            return _rocketStats;
        }

        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
		{
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args, _rocketStats, TurretStats.BurstSize);

            foreach (RocketSpawner rocketSpawner in _rocketSpawners.Items)
			{
                await rocketSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey, _rocketStats);
			}
		}

        public override void Fire(float angleInDegrees)
		{
			_rocketSpawners
                .Next()
                .SpawnRocket(
				    angleInDegrees,
				    transform.IsMirrored(),
				    Target,
				    _targetFilter);
		}
	}
}
