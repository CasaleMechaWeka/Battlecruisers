using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Beams
{
    public interface IBeamCollision
    {
        ITarget Target { get; }
        Vector2 CollisionPoint { get; }
    }
}
