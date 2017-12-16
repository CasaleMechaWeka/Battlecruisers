namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    // Wraps ProjectileStats to hide public fields, so they cannot be inadvertently set.
    public class ProjectileStatsWrapper : DamageStats, IProjectileStats
    {
        public float MaxVelocityInMPerS { get; private set; }
        public bool IgnoreGravity { get; private set; }
        public bool HasAreaOfEffectDamage { get; private set; }
        public float InitialVelocityInMPerS { get; private set; }

        public ProjectileStatsWrapper(ProjectileStats stats)
            : base(stats.damage, stats.damageRadiusInM)
        {
            MaxVelocityInMPerS = stats.maxVelocityInMPerS;
            IgnoreGravity = stats.ignoreGravity;
            HasAreaOfEffectDamage = stats.hasAreaOfEffectDamage;
            InitialVelocityInMPerS = MaxVelocityInMPerS * stats.initialVelocityMultiplier;
        }
    }
}
