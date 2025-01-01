using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public interface IPvPBeamCollision
    {
        ITarget Target { get; }
        Vector2 CollisionPoint { get; }
    }
}
