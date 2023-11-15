using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public class PvPAreaOfEffectDamageApplier : IPvPDamageApplier
    {
        private readonly IPvPDamageStats _damageStats;
        private readonly IPvPTargetFilter _targetFilter;
        private readonly LayerMask _targetLayerMask;

        public PvPAreaOfEffectDamageApplier(IPvPDamageStats damageStats, IPvPTargetFilter targetFilter, LayerMask targetLayerMask = default)
        {
            PvPHelper.AssertIsNotNull(damageStats, targetFilter);

            _damageStats = damageStats;
            _targetFilter = targetFilter;
            _targetLayerMask = targetLayerMask;
        }

        public void ApplyDamage(IPvPTarget baseTarget, Vector2 collisionPoint, IPvPTarget damageSource)
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
                IPvPTarget target = collider.gameObject.GetComponent<IPvPTargetProxy>()?.Target;

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
