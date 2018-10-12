using BattleCruisers.Projectiles.Stats.Wrappers;
using UnityEngine;

namespace BattleCruisers.Projectiles.Stats
{
    public class NukeProjectileStats : CruisingProjectileStats, INukeStats
    {
        public Vector2 InitialVelocity { get { return new Vector2(0, 0); } }
    }
}