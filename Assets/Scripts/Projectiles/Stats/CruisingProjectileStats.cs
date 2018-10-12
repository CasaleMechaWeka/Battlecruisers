using BattleCruisers.Projectiles.Stats.Wrappers;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    public class CruisingProjectileStats : ProjectileStats, ICruisingProjectileStats
    {
        public float cruisingAltitudeInM;
        public float CruisingAltitudeInM { get { return cruisingAltitudeInM; } }

        protected override void OnAwake()
        {
            Assert.IsTrue(cruisingAltitudeInM > 0);
        }
    }
}
