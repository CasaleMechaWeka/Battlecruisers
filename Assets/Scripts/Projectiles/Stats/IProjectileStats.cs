namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileStats : IDamageStats, IProjectileFlight
    {
        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
