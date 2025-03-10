namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileStats : IDamageStats, IProjectileFlightStats
    {
        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
