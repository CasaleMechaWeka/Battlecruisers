namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class LaserTurretStats : TurretStats, ILaserTurretStats
	{
		public float damagePerS;
        public float DamagePerS { get { return damagePerS; } }
		
        public float laserDurationInS;
        public float LaserDurationInS { get { return laserDurationInS; } }
	}
}
