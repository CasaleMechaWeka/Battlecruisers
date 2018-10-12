namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileStats : IDamageStats
    {
        float MaxVelocityInMPerS { get; }
        bool IgnoreGravity { get; }
        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
