using UnityEngine;

namespace BattleCruisers.Projectiles.Stats
{
    public class NukeProjectileStats : CruisingProjectileStats, INukeStats
    {
        public Vector2 InitialVelocity => new Vector2(0, 0);
    }
}