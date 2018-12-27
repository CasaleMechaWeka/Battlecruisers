namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileFlight
    {
        float MaxVelocityInMPerS { get; }
        // FELIX  Convert to float GravityScale :D
        bool IgnoreGravity { get; }
    }
}
