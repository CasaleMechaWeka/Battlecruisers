namespace BattleCruisers.Projectiles.Stats
{
	public abstract class ProjectileStats : IProjectileStats
	{
		public float Damage { get; private set; }
		public float MaxVelocityInMPerS { get; private set; }
		public bool IgnoreGravity { get; private set; }

		public ProjectileStats(float damage, float maxVelocityInMPerS, bool ignoreGravity)
		{
			Damage = damage;
			MaxVelocityInMPerS = maxVelocityInMPerS;
			IgnoreGravity = ignoreGravity;
		}
	}
}
