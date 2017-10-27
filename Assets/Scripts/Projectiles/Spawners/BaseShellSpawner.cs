using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ITargetFilter _targetFilter;

        public void Initialise(IProjectileStats projectileStats, ITargetFilter targetFilter, IDamageApplierFactory damageApplierFactory)
		{
            base.Initialise(projectileStats, damageApplierFactory);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}
	}
}
