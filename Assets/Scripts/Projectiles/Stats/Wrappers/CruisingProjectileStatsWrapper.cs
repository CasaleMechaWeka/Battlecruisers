namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public abstract class CruisingProjectileStatsWrapper : ProjectileStatsWrapper, ICruisingProjectileStats
    {
        public float CruisingAltitudeInM { get; private set; }

        public CruisingProjectileStatsWrapper(CruisingProjectileStats stats)
            : base(stats)
        {
            CruisingAltitudeInM = stats.cruisingAltitudeInM;
        }
    }
}
