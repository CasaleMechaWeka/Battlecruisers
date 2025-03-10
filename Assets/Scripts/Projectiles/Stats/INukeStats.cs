using UnityEngine;

namespace BattleCruisers.Projectiles.Stats
{
    public interface INukeStats : ICruisingProjectileStats
    {
        Vector2 InitialVelocity { get; }
    }
}
