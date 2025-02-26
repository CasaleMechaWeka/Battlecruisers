using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPBombSpawner : PvPProjectileSpawner<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        protected ITargetFilter _targetFilter;

        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, ITargetFilter targetFilter)
        {
            await base.InitialiseAsync(args, firingSound: null);

            Assert.IsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float currentXVelocityInMPers)
        {
            Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            PvPProjectileActivationArgs<IProjectileStats> activationArgs
                = new PvPProjectileActivationArgs<IProjectileStats>(
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
