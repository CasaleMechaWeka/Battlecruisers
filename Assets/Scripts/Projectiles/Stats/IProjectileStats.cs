namespace BattleCruisers.Projectiles.Stats
{
    public interface IProjectileStats
	{
		float Damage { get; }
		float MaxVelocityInMPerS { get; }
		bool IgnoreGravity { get; }
	}
}
