namespace BattleCruisers.Projectiles.TEMP.Wrappers
{
    public abstract class CruisingProjectileStatsWrapper<TPrefab> : ProjectileStatsWrapper<TPrefab> where TPrefab : ProjectileController
    {
        public float CruisingAltitudeInM { get; private set; }

        public CruisingProjectileStatsWrapper(CruisingProjectileStats<TPrefab> stats)
            : base(stats)
        {
            CruisingAltitudeInM = stats.cruisingAltitudeInM;
        }
    }
}
