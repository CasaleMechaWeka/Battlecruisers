using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPMissileSpawner : PvPProjectileSpawner<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, IPvPTarget target, IPvPTargetFilter targetFilter)
        {
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            PvPTargetProviderActivationArgs<IPvPProjectileStats> activationArgs
                = new PvPTargetProviderActivationArgs<IPvPProjectileStats>(
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
