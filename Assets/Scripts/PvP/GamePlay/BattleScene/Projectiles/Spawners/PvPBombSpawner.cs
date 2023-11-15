using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPBombSpawner : PvPProjectileSpawner<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        protected IPvPTargetFilter _targetFilter;

        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, IPvPTargetFilter targetFilter)
        {
            await base.InitialiseAsync(args, firingSound: null);

            Assert.IsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float currentXVelocityInMPers)
        {
            Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            PvPProjectileActivationArgs<IPvPProjectileStats> activationArgs
                = new PvPProjectileActivationArgs<IPvPProjectileStats>(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent,
                    _impactSound);

            base.SpawnProjectile(activationArgs);
        }
    }
}
