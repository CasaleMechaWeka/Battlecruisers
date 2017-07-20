namespace BattleCruisers.Projectiles.Stats
{
    public class ShellStats : ProjectileStats<ProjectileController>
	{
		public ShellStats(ProjectileController shellPrefab, float damage, bool ignoreGravity, float velocityInMPerS)
            : base(shellPrefab, damage, velocityInMPerS, ignoreGravity)
		{
		}
	}
}
