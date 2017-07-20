namespace BattleCruisers.Projectiles.Stats
{
    public class MissileStats : ProjectileStats<MissileController>
	{
        protected override float InitialVelocityMultiplier { get { return 0.5f; } }

		public MissileStats(MissileController missilePrefab, float damage, float maxVelocityInMPerS)
            : base(missilePrefab, damage, maxVelocityInMPerS, ignoreGravity: true)
		{
		}
	}
}
