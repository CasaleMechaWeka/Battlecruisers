using UnityEngine;

namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public class NukeStatsWrapper : CruisingProjectileStatsWrapper
    {
        public Vector2 InitialVelocity { get { return new Vector2(0, 0); } }

        public NukeStatsWrapper(CruisingProjectileStats stats) 
            : base(stats) { }
    }
}
