using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPSmartMissileSpawner : PvPProjectileSpawner<PvPSmartMissileController, ProjectileActivationArgs, ProjectileStats>
    {
        private ProjectileStats _ProjectileStats;

        public async Task InitialiseAsync(
            IPvPProjectileSpawnerArgs args,
            SoundKey firingSound,
            ProjectileStats ProjectileStats)
        {
            Assert.IsNotNull(ProjectileStats);
            _ProjectileStats = ProjectileStats;

            await base.InitialiseAsync(args, firingSound);
        }

        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITargetFilter targetFilter)
        {
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            ProjectileActivationArgs activationArgs
                = new ProjectileActivationArgs(
                    transform.position,
                    _ProjectileStats,
                    missileVelocity,
                    targetFilter,
                    _parent,
                    _impactSound,
                    null,
                    _cruiserSpecificFactories.Targets,
                    _enemyCruiser);

            //  Logging.Log(Tags.PROJECTILE_SPAWNER, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}");

            base.SpawnProjectile(activationArgs);
        }
    }
}
