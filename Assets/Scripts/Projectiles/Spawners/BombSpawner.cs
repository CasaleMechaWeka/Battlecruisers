using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class BombSpawner : ProjectileSpawner<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
    {
        protected ITargetFilter _targetFilter;

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter)
        {
            base.Initialise(args);

            Assert.IsNotNull(targetFilter);
            _targetFilter = targetFilter;
        }

        public void SpawnShell(float currentXVelocityInMPers)
		{
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
            ProjectileActivationArgs<IProjectileStats> activationArgs
                = new ProjectileActivationArgs<IProjectileStats>(
                    transform.position,
                    _projectileStats,
                    shellVelocity,
                    _targetFilter,
                    _parent,
                    _impactSound);
            _projectilePool.GetItem(activationArgs);
		}
	}
}
