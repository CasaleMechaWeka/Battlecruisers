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
    public class SmartMissileSpawner : ProjectileSpawner<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>
	{
        private ISmartProjectileStats _smartProjectileStats;

        public async Task InitialiseAsync(
            IProjectileSpawnerArgs args,
            ISoundKey firingSound,
            ISmartProjectileStats smartProjectileStats)
        {
            Assert.IsNotNull(smartProjectileStats);
            _smartProjectileStats = smartProjectileStats;

            await base.InitialiseAsync(args, firingSound);
        }

        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITargetFilter targetFilter)
		{
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            SmartMissileActivationArgs<ISmartProjectileStats> activationArgs
                = new SmartMissileActivationArgs<ISmartProjectileStats>(
                    transform.position,
                    _smartProjectileStats,
                    missileVelocity,
                    targetFilter,
                    _parent,
                    _impactSound,
                    _cruiserSpecificFactories.Targets,
                    _enemyCruiser);

            Logging.Log(Tags.PROJECTILE_SPAWNER, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}");

            base.SpawnProjectile(activationArgs);
		}
	}
}
