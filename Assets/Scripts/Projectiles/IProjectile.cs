using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public interface IProjectile : IDestructable
    {
        Vector2 Position { get; }
    }
}