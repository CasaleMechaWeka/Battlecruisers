using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public interface ILaserCollision
    {
        ITarget Target { get; }
        Vector2 CollisionPoint { get; }
    }
}
