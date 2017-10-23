namespace BattleCruisers.Projectiles.Stats.Wrappers
{
    public interface IProjectileStats
    {
        float Damage { get; }
        float MaxVelocityInMPerS { get; }
        bool IgnoreGravity { get; }
        bool HasAreaOfEffectDamage { get; }
        float DamageRadiusInM { get; }
        float InitialVelocityInMPerS { get; }
    }
}
