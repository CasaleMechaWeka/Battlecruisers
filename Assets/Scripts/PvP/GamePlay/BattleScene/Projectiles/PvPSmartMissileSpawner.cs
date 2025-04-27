using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPSmartMissileSpawner : PvPProjectileSpawner<PvPSmartMissileController, PvPSmartMissileActivationArgs<SmartProjectileStats>, SmartProjectileStats>
    {
        private SmartProjectileStats _smartProjectileStats;

        public async Task InitialiseAsync(
            IPvPProjectileSpawnerArgs args,
            ISoundKey firingSound,
            SmartProjectileStats smartProjectileStats)
        {
            Assert.IsNotNull(smartProjectileStats);
            _smartProjectileStats = smartProjectileStats;

            await base.InitialiseAsync(args, firingSound);
        }

        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, ITargetFilter targetFilter)
        {
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            PvPSmartMissileActivationArgs<SmartProjectileStats> activationArgs
                = new PvPSmartMissileActivationArgs<SmartProjectileStats>(
                    transform.position,
                    _smartProjectileStats,
                    missileVelocity,
                    targetFilter,
                    _parent,
                    _impactSound,
                    _cruiserSpecificFactories.Targets,
                    _enemyCruiser);

            //  Logging.Log(Tags.PROJECTILE_SPAWNER, $"position: {activationArgs.Position}  initial velocity: {activationArgs.InitialVelocityInMPerS}");

            base.SpawnProjectile(activationArgs);
        }
    }
}
