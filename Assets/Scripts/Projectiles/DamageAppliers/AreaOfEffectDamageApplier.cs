using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public class AreaOfEffectDamageApplier : IDamageApplier
    {
        private readonly IDamageStats _damageStats;
        private readonly ITargetFilter _targetFilter;
        private readonly LayerMask _targetLayerMask;

        // Using a HashSet collection type as it is an unordered collection of UNIQUE elements.
        // This is important for dealing with multiple colliders belonging to the same unit/cruiser.
        private HashSet<ITarget> damagedTargets = new HashSet<ITarget>();

        public AreaOfEffectDamageApplier(IDamageStats damageStats, ITargetFilter targetFilter, LayerMask targetLayerMask = default)
        {
            Helper.AssertIsNotNull(damageStats, targetFilter);

            _damageStats = damageStats;
            _targetFilter = targetFilter;
            _targetLayerMask = targetLayerMask;
        }

        public void ApplyDamage(ITarget baseTarget, Vector2 collisionPoint, ITarget damageSource)
        {
            Collider2D[] colliders;

            if (_targetLayerMask == default)
            {
                colliders = Physics2D.OverlapCircleAll(collisionPoint, _damageStats.DamageRadiusInM);
            }
            else
            {
                colliders = Physics2D.OverlapCircleAll(collisionPoint, _damageStats.DamageRadiusInM, _targetLayerMask);
			}

            foreach (Collider2D collider in colliders)
            {
				ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;
                
                // damagedTargets is a set that is populated when damage is dealt to a collider.
                // This is specifically useful for the HuntressPrime boss that has multiple colliders
                // close together, which can oause AOE damage to deal multiple times more health than intended.
                if (target != null
                    && !target.IsDestroyed
                    && _targetFilter.IsMatch(target)
                    && !damagedTargets.Contains(target))
                {
                    target.TakeDamage(_damageStats.Damage, damageSource);
                    damagedTargets.Add(target);
                }
            }

            // Clear the set of damaged targets for the next call to ApplyDamage()
            damagedTargets.Clear();
        }
    }
}
