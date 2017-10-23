using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ITargetFilter _targetFilter;

        public void Initialise(IProjectileStats projectileStats, ITargetFilter targetFilter)
		{
            base.Initialise(projectileStats);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}
	}
}
