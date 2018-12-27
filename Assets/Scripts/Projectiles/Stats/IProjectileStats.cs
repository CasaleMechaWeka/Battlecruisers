namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileStats : IDamageStats, IFlightStats
    {
        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
