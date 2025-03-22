using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public class DamageApplierFactory : IDamageApplierFactory
    {
        public IDamageStats CreateDamageStats(float damage, float damageRadiusInM)
        {
            return new DamageStats(damage, damageRadiusInM);
        }

        public IDamageApplier CreateSingleDamageApplier(IDamageStats damageStats)
        {
            return new SingleDamageApplier(damageStats.Damage);
        }

        /// <summary>
        /// All ITargets are susceptible to area of effect damage, 
        /// regardless of faction (ie, friendly fire is on :) ).
        /// </summary>
        public IDamageApplier CreateAreaOfDamageApplier(IDamageStats damageStats)
        {
            ITargetFilter damageTargetFilter = TargetFilterFactory.CreateDummyTargetFilter(isMatchResult: true);
            return new AreaOfEffectDamageApplier(damageStats, damageTargetFilter);
        }

        /// <summary>
        /// Only targets that match the given "enemyFaction" are damaged.
        /// Ie, friendly fire is off :).
        /// </summary>
        public IDamageApplier CreateFactionSpecificAreaOfDamageApplier(IDamageStats damageStats, Faction enemyFaction)
        {
            ITargetFilter damageTargetFilter = TargetFilterFactory.CreateTargetFilter(enemyFaction);
            return new AreaOfEffectDamageApplier(damageStats, damageTargetFilter);
        }
    }
}
