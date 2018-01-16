using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
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
