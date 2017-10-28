using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class RocketSpawner : ProjectileSpawner
	{
        private ICruisingProjectileStats _rocketStats;

        public RocketController rocketPrefab;
        protected override ProjectileController ProjectilePrefab { get { return rocketPrefab; } }

        public void Initialise(ICruisingProjectileStats rocketStats, IFactoryProvider factoryProvider)
		{
            base.Initialise(rocketStats, factoryProvider);

            _rocketStats = rocketStats;
		}

		public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter, Faction faction)
		{
            RocketController rocket = Instantiate(rocketPrefab, transform.position, new Quaternion());
            Vector2 rocketVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
            rocket.Initialise(_rocketStats, rocketVelocity, targetFilter, target, _factoryProvider, faction);
		}
	}
}
