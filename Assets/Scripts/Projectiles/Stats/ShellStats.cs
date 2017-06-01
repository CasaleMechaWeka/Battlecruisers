using System;

namespace BattleCruisers.Projectiles.Stats
{
	public class ShellStats : ProjectileStats
	{
		public ProjectileController ShellPrefab { get; private set; }

		public ShellStats(ProjectileController shellPrefab, float damage, bool ignoreGravity, float velocityInMPerS)
			: base(damage, velocityInMPerS, ignoreGravity)
		{
			ShellPrefab = shellPrefab;
		}
	}
}
