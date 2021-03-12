using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class SmartMissileSpawner : ProjectileSpawner<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>
	{
		public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
		{
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            SmartMissileActivationArgs<ISmartProjectileStats> activationArgs
                = new SmartMissileActivationArgs<ISmartProjectileStats>(
                    transform.position,
                    // FELIX 
                    null,
                    //_projectileStats,
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
