using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    public class CruisingProjectileStats : ProjectileStats, ICruisingProjectileStats
    {
        public float cruisingAltitudeInM;
        public float CruisingAltitudeInM => cruisingAltitudeInM;

        [Header("Turret stats accuracy is ignored. The accuracy is binary :)")]
        public bool isAccurate = true;
        public bool IsAccurate => isAccurate;

        protected override void OnAwake()
        {
            Assert.IsTrue(cruisingAltitudeInM > 0);
        }
    }
}
