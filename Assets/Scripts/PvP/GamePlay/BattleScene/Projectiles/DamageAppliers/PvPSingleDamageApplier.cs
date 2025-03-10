using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.DamageAppliers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public class PvPSingleDamageApplier : IDamageApplier
    {
        private readonly float _damage;

        public PvPSingleDamageApplier(float damage)
        {
            _damage = damage;
        }

        public void ApplyDamage(ITarget target, Vector2 collisionPoint, ITarget damageSource)
        {
            target.TakeDamage(_damage, damageSource);
        }
    }
}
