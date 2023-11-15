using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public class PvPDamageApplierFactory : IPvPDamageApplierFactory
    {
        private readonly IPvPTargetFilterFactory _filterFacotry;

        public PvPDamageApplierFactory(IPvPTargetFilterFactory filterFactory)
        {
            Assert.IsNotNull(filterFactory);
            _filterFacotry = filterFactory;
        }

        public IPvPDamageStats CreateDamageStats(float damage, float damageRadiusInM)
        {
            return new PvPDamageStats(damage, damageRadiusInM);
        }

        public IPvPDamageApplier CreateSingleDamageApplier(IPvPDamageStats damageStats)
        {
            return new PvPSingleDamageApplier(damageStats.Damage);
        }

        /// <summary>
        /// All ITargets are susceptible to area of effect damage, 
        /// regardless of faction (ie, friendly fire is on :) ).
        /// </summary>
        public IPvPDamageApplier CreateAreaOfDamageApplier(IPvPDamageStats damageStats)
        {
            IPvPTargetFilter damageTargetFilter = _filterFacotry.CreateDummyTargetFilter(isMatchResult: true);
            return new PvPAreaOfEffectDamageApplier(damageStats, damageTargetFilter);
        }

        /// <summary>
        /// Only targets that match the given "enemyFaction" are damaged.
        /// Ie, friendly fire is off :).
        /// </summary>
        public IPvPDamageApplier CreateFactionSpecificAreaOfDamageApplier(IPvPDamageStats damageStats, PvPFaction enemyFaction)
        {
            IPvPTargetFilter damageTargetFilter = _filterFacotry.CreateTargetFilter(enemyFaction);
            return new PvPAreaOfEffectDamageApplier(damageStats, damageTargetFilter);
        }
    }
}
