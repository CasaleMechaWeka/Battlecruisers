namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileFlightStats
    {
        float MaxVelocityInMPerS { get; }
        // FELIX  Convert to float GravityScale :D
        bool IgnoreGravity { get; }
    }
}
