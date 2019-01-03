using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.Factories;
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
            ITargetFilter damageTargetFilter = _targetsFactory.CreateDummyTargetFilter(isMatchResult: true);
            return new AreaOfEffectDamageApplier(damageStats, damageTargetFilter);
        }

        /// <summary>
        /// Only targets that match the given "enemyFaction" are damaged.
        /// Ie, friendly fire is off :).
        /// </summary>
        public IDamageApplier CreateFactionSpecificAreaOfDamageApplier(IDamageStats damageStats, Faction enemyFaction)
        {
            ITargetFilter damageTargetFilter = _targetsFactory.CreateTargetFilter(enemyFaction);
            return new AreaOfEffectDamageApplier(damageStats, damageTargetFilter);
        }
    }
}
