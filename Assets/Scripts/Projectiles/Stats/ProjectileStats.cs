namespace BattleCruisers.Projectiles.Stats
{
    public abstract class ProjectileStats<TPrefab> : IProjectileStats where TPrefab : ProjectileController
    {
		protected virtual float InitialVelocityMultiplier { get { return 1; } }

        public TPrefab ProjectilePrefab { get; private set; }
        public float Damage { get; private set; }
        public float MaxVelocityInMPerS { get; private set; }
        public float InitialVelocityInMPerS { get { return MaxVelocityInMPerS * InitialVelocityMultiplier; } }
        public bool IgnoreGravity { get; private set; }
        public float DamageRadiusInM { get; private set; }

        public ProjectileStats(TPrefab projectilePrefab, float damage, float maxVelocityInMPerS, bool ignoreGravity, float damageRadiusInM = 0)
		{
            ProjectilePrefab = projectilePrefab;
			Damage = damage;
			MaxVelocityInMPerS = maxVelocityInMPerS;
			IgnoreGravity = ignoreGravity;
            DamageRadiusInM = damageRadiusInM;
		}
	}
}
