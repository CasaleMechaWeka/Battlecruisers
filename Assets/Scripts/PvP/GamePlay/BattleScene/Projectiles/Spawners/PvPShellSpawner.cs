using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public class PvPShellSpawner : PvPProjectileSpawner<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>, IPvPProjectileStats>
    {
        private IPvPTargetFilter _targetFilter;

        public async Task InitialiseAsync(IPvPProjectileSpawnerArgs args, IPvPSoundKey firingSound, IPvPTargetFilter targetFilter)
        {
            await base.InitialiseAsync(args, firingSound);

            PvPHelper.AssertIsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
        {
            Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
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
