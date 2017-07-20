namespace BattleCruisers.Projectiles.Stats
{
    public class RocketStats : CruisingProjectileStats<RocketController>
	{
		protected override float InitialVelocityMultiplier { get { return 0.15f; } }

		public RocketStats(RocketController rocketPrefab, float damage, float maxVelocityInMPerS, float cruisingAltitudeInM)
            : base(rocketPrefab, damage, maxVelocityInMPerS, cruisingAltitudeInM)
		{
		}
	}
}
