using BattleCruisers.Buildables;
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
            ITarget parent,
            IProjectileStats projectileStats, 
            ITargetFilter targetFilter, 
            IFactoryProvider factoryProvider)
		{
            base.Initialise(parent, projectileStats, factoryProvider);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}
	}
}
