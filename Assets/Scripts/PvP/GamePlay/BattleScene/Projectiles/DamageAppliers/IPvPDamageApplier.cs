using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers
{
    public interface IPvPDamageApplier
    {
        void ApplyDamage(IPvPTarget target, Vector2 collisionPoint, IPvPTarget damageSource);
    }
}
