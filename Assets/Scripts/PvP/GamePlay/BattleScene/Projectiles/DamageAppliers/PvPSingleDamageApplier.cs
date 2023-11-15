using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public class PvPSingleDamageApplier : IPvPDamageApplier
    {
        private readonly float _damage;

        public PvPSingleDamageApplier(float damage)
        {
            _damage = damage;
        }

        public void ApplyDamage(IPvPTarget target, Vector2 collisionPoint, IPvPTarget damageSource)
        {
            target.TakeDamage(_damage, damageSource);
        }
    }
}
