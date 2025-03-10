using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public class PvPAreaOfEffectDamageApplier : IDamageApplier
    {
        private readonly IDamageStats _damageStats;
        private readonly ITargetFilter _targetFilter;
        private readonly LayerMask _targetLayerMask;

        public PvPAreaOfEffectDamageApplier(IDamageStats damageStats, ITargetFilter targetFilter, LayerMask targetLayerMask = default)
        {
            PvPHelper.AssertIsNotNull(damageStats, targetFilter);

            _damageStats = damageStats;
            _targetFilter = targetFilter;
            _targetLayerMask = targetLayerMask;
        }

        public void ApplyDamage(ITarget baseTarget, Vector2 collisionPoint, ITarget damageSource)
        {
            Collider2D[] colliders;
            Collider2D[] secondaryColliders;

            if (_targetLayerMask == default)
            {
                colliders = Physics2D.OverlapCircleAll(collisionPoint, _damageStats.DamageRadiusInM);
                secondaryColliders = Physics2D.OverlapCircleAll(collisionPoint, _damageStats.SecondaryRadiusInM);
            }
            else
            {
                colliders = Physics2D.OverlapCircleAll(collisionPoint, _damageStats.DamageRadiusInM, _targetLayerMask);
                secondaryColliders = Physics2D.OverlapCircleAll(collisionPoint, _damageStats.SecondaryRadiusInM, _targetLayerMask);
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


            foreach (Collider2D collider in secondaryColliders)
            {
                ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;

                if (target != null
                    && !target.IsDestroyed
                    && _targetFilter.IsMatch(target))
                {
                    target.TakeDamage(_damageStats.SecondaryDamage, damageSource);
                }
            }
        }
    }
}
