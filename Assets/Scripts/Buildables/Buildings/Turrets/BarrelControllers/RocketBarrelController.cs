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
        private ICruisingProjectileStats _rocketStats;

        public override Vector3 ProjectileSpawnerPosition => _rocketSpawners.Items.Middle().transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            RocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<RocketSpawner>();
            Assert.IsTrue(rocketSpawners.Length != 0);
            _rocketSpawners = new CircularList<RocketSpawner>(rocketSpawners);
        }

        protected override IProjectileStats GetProjectileStats()
        {
            _rocketStats = GetComponent<CruisingProjectileStats>();
			Assert.IsNotNull(_rocketStats);
            return _rocketStats;
        }

#pragma warning disable 1998  // This async method lacks 'await' operators and will run synchronously
        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
		{
			foreach (RocketSpawner rocketSpawner in _rocketSpawners.Items)
			{
                rocketSpawner.Initialise(args.Parent, _rocketStats, TurretStats.BurstSize, args.FactoryProvider);
			}
		}
#pragma warning restore 1998  // This async method lacks 'await' operators and will run synchronously

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
