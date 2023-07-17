using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Projectiles.ActivationArgs;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPSmartMissileSpawner : PvPProjectileSpawner<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>, IPvPSmartProjectileStats>
    {
        private IPvPSmartProjectileStats _smartProjectileStats;

        public async Task InitialiseAsync(
            IPvPProjectileSpawnerArgs args,
            IPvPSoundKey firingSound,
            IPvPSmartProjectileStats smartProjectileStats)
        {
            Assert.IsNotNull(smartProjectileStats);
            _smartProjectileStats = smartProjectileStats;

            await base.InitialiseAsync(args, firingSound);
        }

        public void SpawnMissile(float angleInDegrees, bool isSourceMirrored, IPvPTargetFilter targetFilter)
        {
            Vector2 missileVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.InitialVelocityInMPerS);
            PvPSmartMissileActivationArgs<IPvPSmartProjectileStats> activationArgs
                = new PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>(
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
