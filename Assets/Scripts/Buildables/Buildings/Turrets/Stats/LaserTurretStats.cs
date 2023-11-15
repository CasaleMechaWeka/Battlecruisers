using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class LaserTurretStats : TurretStats, ILaserTurretStats
	{
		public float damagePerS;
        public float DamagePerS => damagePerS;
		
        public float laserDurationInS;
        public float LaserDurationInS => laserDurationInS;

        public override void ApplyVariantStats(StatVariant statVariant)
        {
            if(!isAppliedVariant)
            {
                base.ApplyVariantStats(statVariant);
                damagePerS += statVariant.damagePerS;
                laserDurationInS += statVariant.laser_duration;
            }
        }
    }
}
