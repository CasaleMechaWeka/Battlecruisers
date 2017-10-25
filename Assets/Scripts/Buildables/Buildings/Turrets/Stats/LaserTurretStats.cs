namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class LaserTurretStats : TurretStats
	{
        // FELIX  Create interface for all turret stats, and expose properties.
        // Means fields cannot be set by accident.
		public float damagePerS;
		public float laserDurationInS;
	}
}
