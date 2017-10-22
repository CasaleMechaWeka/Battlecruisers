namespace BattleCruisers.Projectiles.TEMP.Wrappers
{
    public interface IProjectileStats<TPrefab> where TPrefab : ProjectileController
    {
        TPrefab ProjectilePrefab { get; }
        float Damage { get; }
        float MaxVelocityInMPerS { get; }
        bool IgnoreGravity { get; }
        bool HasAreaOfEffectDamage { get; }
        float DamageRadiusInM { get; }
        float InitialVelocityInMPerS { get; }
    }
}
