using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>
	{
		protected ITargetFilter _targetFilter;

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter)
		{
            base.Initialise(args);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}
	}
}
