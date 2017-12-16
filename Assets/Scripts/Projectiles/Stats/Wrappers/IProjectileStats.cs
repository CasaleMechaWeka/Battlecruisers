namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public interface IProjectileStats : IDamageStats
    {
        float MaxVelocityInMPerS { get; }
        bool IgnoreGravity { get; }
        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
