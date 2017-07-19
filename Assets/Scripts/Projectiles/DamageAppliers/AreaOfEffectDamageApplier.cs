using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Projectiles.DamageAppliers
{
    public class AreaOfEffectDamageApplier : IDamageApplier
    {
        private readonly float _damage;
        private readonly float _radiusInM;
        private readonly ITargetFilter _targetFilter;
        private readonly LayerMask _targetLayerMask;


        public AreaOfEffectDamageApplier(float damage, float radiusInM, ITargetFilter targetFilter, LayerMask targetLayerMask = default(LayerMask))
        {
            _damage = damage;
            _radiusInM = radiusInM;
            _targetFilter = targetFilter;
            _targetLayerMask = targetLayerMask;
        }

        public void ApplyDamage(ITarget baseTarget)
        {
            Collider2D[] colliders;

            if (_targetLayerMask == default(LayerMask))
            {
				colliders = Physics2D.OverlapCircleAll(baseTarget.Position, _radiusInM);
            }
            else
            {
                colliders = Physics2D.OverlapCircleAll(baseTarget.Position, _radiusInM, _targetLayerMask);
			}

            foreach (Collider2D collider in colliders)
            {
				ITarget target = collider.gameObject.GetComponent<ITarget>();

                if (target != null && _targetFilter.IsMatch(target))
                {
                    target.TakeDamage(_damage);
                }
            }
		}
    }
}
