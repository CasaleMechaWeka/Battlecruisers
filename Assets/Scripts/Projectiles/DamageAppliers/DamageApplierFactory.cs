using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public class DamageApplierFactory : IDamageApplierFactory
    {
        private readonly ITargetsFactory _targetsFactory;

        public DamageApplierFactory(ITargetsFactory targetsFactory)
        {
            Assert.IsNotNull(targetsFactory);
            _targetsFactory = targetsFactory;
        }

        public IDamageApplier CreateSingleDamageApplier(IProjectileStats projectileStats)
        {
            return new SingleDamageApplier(projectileStats.Damage);
        }

        public IDamageApplier CreateAreaOfDamageApplier(IProjectileStats projectileStats)
        {
            // All ITargets are susceptible to area of effect damage
            ITargetFilter damageTargetFilter = _targetsFactory.CreateDummyTargetFilter(isMatchResult: true);
            return new AreaOfEffectDamageApplier(projectileStats.Damage, projectileStats.DamageRadiusInM, damageTargetFilter);
        }
    }
}
