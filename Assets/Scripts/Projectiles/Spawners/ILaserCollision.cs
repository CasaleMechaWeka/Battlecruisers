using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
    public interface ILaserCollision
    {
        ITarget Target { get; }
        Vector2 CollisionPoint { get; }
    }
}
