using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class RocketSpawner : ProjectileSpawner
	{
        private ICruisingProjectileStats _rocketStats;

        public RocketController rocketPrefab;
        protected override MonoBehaviour ProjectilePrefab => rocketPrefab;

        public void Initialise(ITarget parent, ICruisingProjectileStats rocketStats, int burstSize, IFactoryProvider factoryProvider)
		{
            base.Initialise(new ProjectileSpawnerArgs(parent, rocketStats, burstSize, factoryProvider));

            _rocketStats = rocketStats;
		}

		public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter, Faction faction)
		{
            RocketController rocket = Instantiate(rocketPrefab, transform.position, new Quaternion());
            Vector2 rocketVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
            rocket.Initialise(_rocketStats, rocketVelocity, targetFilter, target, _factoryProvider, _parent, faction);
		}
	}
}
