using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPRocketSpawner : PvPProjectileSpawner<PvPRocketController, TargetProviderActivationArgs<CruisingProjectileStats>, CruisingProjectileStats>
    {
        private CruisingProjectileStats _rocketStats;

        public async Task InitialiseAsync(
            IPvPProjectileSpawnerArgs args,
            ISoundKey firingSound,
            CruisingProjectileStats rocketStats)
        {
            Assert.IsNotNull(rocketStats);
            _rocketStats = rocketStats;

            await base.InitialiseAsync(args, firingSound);
        }

        public void SpawnRocket(float angleInDegrees, bool isSourceMirrored, ITarget target, ITargetFilter targetFilter)
        {
            // Logging.Log(Tags.PROJECTILE_SPAWNER, $"spawn position: {transform.position}");

            Vector2 rocketVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _rocketStats.InitialVelocityInMPerS);
            TargetProviderActivationArgs<CruisingProjectileStats> activationArgs
                = new TargetProviderActivationArgs<CruisingProjectileStats>(
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
