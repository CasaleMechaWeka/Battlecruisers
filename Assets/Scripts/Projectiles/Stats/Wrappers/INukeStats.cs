using UnityEngine;

namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public interface INukeProjectileStats : ICruisingProjectileStats
    {
        Vector2 InitialVelocity { get; }
    }
}
