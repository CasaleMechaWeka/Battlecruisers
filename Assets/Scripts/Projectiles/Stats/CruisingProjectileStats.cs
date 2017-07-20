namespace BattleCruisers.Projectiles.Stats
{
    public class CruisingProjectileStats<TPrefab> : ProjectileStats<TPrefab> where TPrefab : ProjectileController
	{
		public float CruisingAltitudeInM { get; private set; }

        public CruisingProjectileStats(TPrefab projectilePrefab, float damage, float maxVelocityInMPerS, float cruisingAltitudeInM, float damageRadiusInM = 0)
            : base(projectilePrefab, damage, maxVelocityInMPerS, ignoreGravity: true, damageRadiusInM: damageRadiusInM)
		{
			CruisingAltitudeInM = cruisingAltitudeInM;
		}
	}
}
