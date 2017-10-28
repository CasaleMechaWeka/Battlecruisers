using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ITargetFilter _targetFilter;

        public void Initialise(
            IProjectileStats projectileStats, 
            ITargetFilter targetFilter, 
            IFactoryProvider factoryProvider)
		{
            base.Initialise(projectileStats, factoryProvider);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}
	}
}
