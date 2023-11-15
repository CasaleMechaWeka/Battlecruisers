using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public class PvPCruisingProjectileStats : PvPProjectileStats, IPvPCruisingProjectileStats
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
