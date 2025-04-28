using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPMissileSpawner : PvPProjectileSpawner<PvPMissileController, ProjectileActivationArgs, ProjectileStats>
    {
        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
        {
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            ProjectileActivationArgs activationArgs
                = new ProjectileActivationArgs(
                    transform.position,
                    _projectileStats,
                    missileVelocity,
                    targetFilter,
                    _parent,
                    _impactSound,
                    target);

            // Logging.Log(Tags.PROJECTILE_SPAWNER, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}");

            base.SpawnProjectile(activationArgs);
        }
    }
}
