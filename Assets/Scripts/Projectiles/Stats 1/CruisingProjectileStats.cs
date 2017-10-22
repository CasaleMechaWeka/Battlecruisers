using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.TEMP
{
    public abstract class CruisingProjectileStats<TPrefab> : ProjectileStats<TPrefab> where TPrefab : ProjectileController
    {
        public float cruisingAltitudeInM;

        protected override void OnAwake()
        {
            Assert.IsTrue(cruisingAltitudeInM > 0);
        }
    }
}
