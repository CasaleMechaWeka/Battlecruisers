namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileStats : IDamageStats
    {
        // FELIX  Move these 2 to IProjectileFlightStats => Needed by angle calculators :)
        float MaxVelocityInMPerS { get; }
        bool IgnoreGravity { get; }

        bool HasAreaOfEffectDamage { get; }
        float InitialVelocityInMPerS { get; }
    }
}
