using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Proxy;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public class AreaOfEffectDamageApplier : IDamageApplier
    {
        private readonly IDamageStats _damageStats;
        private readonly ITargetFilter _targetFilter;
        private readonly LayerMask _targetLayerMask;

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

                if (target != null 
                    && !target.IsDestroyed
                    && _targetFilter.IsMatch(target))
                {
                    target.TakeDamage(_damageStats.Damage, damageSource);
                }
            }
		}
    }
}
