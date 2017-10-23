using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    public abstract class CruisingProjectileStats : ProjectileStats
    {
        public float cruisingAltitudeInM;

        protected override void OnAwake()
        {
            Assert.IsTrue(cruisingAltitudeInM > 0);
        }
    }
}
