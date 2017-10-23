namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public class CruisingProjectileStatsWrapper : ProjectileStatsWrapper, ICruisingProjectileStats
    {
        public float CruisingAltitudeInM { get; private set; }

        public CruisingProjectileStatsWrapper(CruisingProjectileStats stats)
            : base(stats)
        {
            CruisingAltitudeInM = stats.cruisingAltitudeInM;
        }

        // FELIX  Get rid of all these stupid constructors?  If test cases can create base Stats classes in inspector :)
        public CruisingProjectileStatsWrapper(
            float damage,
            float maxVelocityInMPerS,
            bool ignoreGravity,
            bool hasAreaOfEffectDamage,
            float damageRadiusInM,
            float initialVelocityMultiplier,
            float cruisingAltitudeInM)
            : base(
                damage,
                maxVelocityInMPerS,
                ignoreGravity,
                hasAreaOfEffectDamage,
                damageRadiusInM,
                initialVelocityMultiplier)
        {
            CruisingAltitudeInM = cruisingAltitudeInM;
        }
    }
}
