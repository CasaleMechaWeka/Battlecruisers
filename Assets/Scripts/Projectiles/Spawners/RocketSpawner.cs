using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class RocketSpawner : ProjectileSpawner<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>
	{
        private ICruisingProjectileStats _rocketStats;

        public async Task InitialiseAsync(
            IProjectileSpawnerArgs args,
            ISoundKey firingSound,
            ICruisingProjectileStats rocketStats)
		{
            Assert.IsNotNull(rocketStats);
            _rocketStats = rocketStats;

            await base.InitialiseAsync(args, firingSound);
		}

		public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
            Logging.Log(Tags.PROJECTILE_SPAWNER, $"spawn position: {transform.position}");

            Vector2 rocketVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
            TargetProviderActivationArgs<ICruisingProjectileStats> activationArgs
                = new TargetProviderActivationArgs<ICruisingProjectileStats>(
                    transform.position,
                    _rocketStats,
                    rocketVelocity,
                    targetFilter,
                    _parent,
                    _impactSound,
                    target);
            base.SpawnProjectile(activationArgs);
		}
	}
}
