using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public interface IPvPBeamCollision
    {
        IPvPTarget Target { get; }
        Vector2 CollisionPoint { get; }
    }
}
