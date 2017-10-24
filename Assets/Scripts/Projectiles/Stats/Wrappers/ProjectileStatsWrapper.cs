namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    // Wraps ProjectileStats to hide public fields, so they cannot be inadvertently set.
    public class ProjectileStatsWrapper : IProjectileStats
    {
        public float Damage { get; private set; }
        public float MaxVelocityInMPerS { get; private set; }
        public bool IgnoreGravity { get; private set; }
        public bool HasAreaOfEffectDamage { get; private set; }
        public float DamageRadiusInM { get; private set; }
        public float InitialVelocityInMPerS { get; private set; }

        public ProjectileStatsWrapper(ProjectileStats stats)
        {
            Damage = stats.damage;
            MaxVelocityInMPerS = stats.maxVelocityInMPerS;
            IgnoreGravity = stats.ignoreGravity;
            HasAreaOfEffectDamage = stats.hasAreaOfEffectDamage;
            DamageRadiusInM = stats.damageRadiusInM;
            InitialVelocityInMPerS = MaxVelocityInMPerS * stats.initialVelocityMultiplier;
        }
    }
}
