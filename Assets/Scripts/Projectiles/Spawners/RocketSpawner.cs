using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class RocketSpawner : ProjectileSpawner<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
	{
        private ICruisingProjectileStats _rocketStats;

        public RocketController rocketPrefab;
        protected override MonoBehaviour ProjectilePrefab => rocketPrefab;

        public void Initialise(ITarget parent, ICruisingProjectileStats rocketStats, int burstSize, IFactoryProvider factoryProvider)
		{
            base.Initialise(new ProjectileSpawnerArgs(parent, rocketStats, burstSize, factoryProvider));

            _rocketStats = rocketStats;
		}

		public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
            Vector2 rocketVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
            TargetProviderActivationArgs<ICruisingProjectileStats> activationArgs
                = new TargetProviderActivationArgs<ICruisingProjectileStats>(
                    transform.position,
                    _rocketStats,
                    rocketVelocity,
                    targetFilter,
                    _parent,
                    target);
            _projectilePool.GetItem(activationArgs);
		}
	}
}
