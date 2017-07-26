namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class LaserTurretStats : TurretStats
	{
		public float damagePerS;
		public float laserDurationInS;

		public override float DamagePerS { get { return damagePerS; } }
	}
}
