using UnityEngine;

namespace BattleCruisers.Projectiles.TEMP.Wrappers
{
    public class NukeStatsWrapper : CruisingProjectileStatsWrapper<NukeController>
    {
        public Vector2 InitialVelocity { get { return new Vector2(0, 0); } }

        public NukeStatsWrapper(CruisingProjectileStats<NukeController> stats) 
            : base(stats) { }
    }
}
