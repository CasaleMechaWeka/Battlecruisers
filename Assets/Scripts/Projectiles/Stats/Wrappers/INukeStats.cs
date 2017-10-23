using UnityEngine;

namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public interface INukeStats : ICruisingProjectileStats
    {
        Vector2 InitialVelocity { get; }
    }
}
