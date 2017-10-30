using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class RocketBarrelController : BarrelController
	{
		private ICircularList<RocketSpawner> _rocketSpawners;
		private Faction _faction;
        private ICruisingProjectileStats _rocketStats;

        protected override Vector3 ProjectileSpawnerPosition { get { return _rocketSpawners.Items.Middle().transform.position; } }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            RocketSpawner[] rocketSpawners = gameObject.GetComponentsInChildren<RocketSpawner>();
            Assert.IsTrue(rocketSpawners.Length != 0);
            _rocketSpawners = new CircularList<RocketSpawner>(rocketSpawners);
        }

        protected override IProjectileStats GetProjectileStats()
        {
            CruisingProjectileStats rocketStats = GetComponent<CruisingProjectileStats>();
			Assert.IsNotNull(rocketStats);
            _rocketStats = new CruisingProjectileStatsWrapper(rocketStats);
            return _rocketStats;
        }

		public void Initialise(IBarrelControllerArgs args, Faction faction)
		{
            base.Initialise(args);

			_faction = faction;

			foreach (RocketSpawner rocketSpawner in _rocketSpawners.Items)
			{
                rocketSpawner.Initialise(_rocketStats, args.FactoryProvider);
			}
		}

		protected override void Fire(float angleInDegrees)
		{
			_rocketSpawners.Next().SpawnRocket(
				angleInDegrees,
				transform.IsMirrored(),
				Target,
				_targetFilter,
				_faction);
		}
	}
}
