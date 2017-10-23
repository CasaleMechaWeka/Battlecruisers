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

        public ProjectileStatsWrapper(ProjectileStats stats) : this(
            stats.damage,
            stats.maxVelocityInMPerS,
            stats.ignoreGravity,
            stats.hasAreaOfEffectDamage,
            stats.damageRadiusInM,
            stats.initialVelocityMultiplier)
        { }

        public ProjectileStatsWrapper(
            float damage,
            float maxVelocityInMPerS,
            bool ignoreGravity,
            bool hasAreaOfEffectDamage,
            float damageRadiusInM,
            float initialVelocityMultiplier)
        {
            Damage = damage;
            MaxVelocityInMPerS = maxVelocityInMPerS;
            IgnoreGravity = ignoreGravity;
            HasAreaOfEffectDamage = hasAreaOfEffectDamage;
            DamageRadiusInM = damageRadiusInM;
            InitialVelocityInMPerS = MaxVelocityInMPerS * initialVelocityMultiplier;
        }
    }
}
