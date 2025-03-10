using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public class PvPLaserTurretStats : PvPTurretStats, ILaserTurretStats
    {
        public float damagePerS;
        public float DamagePerS => damagePerS;

        public float laserDurationInS;
        public float LaserDurationInS => laserDurationInS;

        public override void ApplyVariantStats(StatVariant statVariant)
        {
            if (!isAppliedVariant)
            {
                base.ApplyVariantStats(statVariant);
                damagePerS += statVariant.damagePerS;
                laserDurationInS += statVariant.laser_duration;
            }
        }
    }
}
