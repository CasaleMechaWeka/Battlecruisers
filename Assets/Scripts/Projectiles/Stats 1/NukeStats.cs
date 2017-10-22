using UnityEngine;

namespace BattleCruisers.Projectiles.Stats.TEMP
{
    public class NukeStats : CruisingProjectileStats<NukeController>
    {
        // FELIX  Move to wrapper?
        public Vector2 InitialVelocity { get { return new Vector2(0, 0); } }
    }
}
