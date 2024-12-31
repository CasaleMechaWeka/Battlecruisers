using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPTurretStats : PvPBasicTurretStats, ITurretStats
    {
        public float accuracy;
        public float Accuracy => accuracy;

        public float turretRotateSpeedInDegrees;
        public float TurretRotateSpeedInDegrees => turretRotateSpeedInDegrees;

        public virtual bool IsInBurst => false;

        public virtual int BurstSize => DEFAULT_BURST_SIZE;

        private const int DEFAULT_BURST_SIZE = 1;

        public override void Initialise()
        {
            base.Initialise();

            Assert.IsTrue(accuracy >= 0 && accuracy <= 1);
            Assert.IsTrue(turretRotateSpeedInDegrees > 0);
        }

        public override void ApplyVariantStats(StatVariant statVariant)
        {
            if (!isAppliedVariant)
            {
                base.ApplyVariantStats(statVariant);
                accuracy += statVariant.accuracy;
                accuracy = Mathf.Clamp01(accuracy);
                turretRotateSpeedInDegrees += statVariant.rotate_speed;
                turretRotateSpeedInDegrees = turretRotateSpeedInDegrees <= 0 ? 0.1f : turretRotateSpeedInDegrees;
            }
        }
    }
}
