using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public class ShellSpawner : ProjectileSpawner<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        private ITargetFilter _targetFilter;

        public async Task InitialiseAsync(IProjectileSpawnerArgs args, ISoundKey firingSound, ITargetFilter targetFilter)
        {
            await base.InitialiseAsync(args, firingSound);

            Helper.AssertIsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            ProjectileActivationArgs<IProjectileStats> activationArgs
                = new ProjectileActivationArgs<IProjectileStats>(
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
