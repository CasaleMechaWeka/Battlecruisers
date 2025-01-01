using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public interface IPvPDamageApplier
    {
        void ApplyDamage(ITarget target, Vector2 collisionPoint, ITarget damageSource);
    }
}
