namespace BattleCruisers.Projectiles.Stats
{
    public interface ICruisingProjectileStats : IProjectileStats
    {
        float CruisingAltitudeInM { get; }
        bool IsAccurate { get; }
    }
}
