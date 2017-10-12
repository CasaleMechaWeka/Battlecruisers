using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ShellStats _shellStats;
		protected ITargetFilter _targetFilter;

		public void Initialise(ShellStats shellStats, ITargetFilter targetFilter)
		{
			_shellStats = shellStats;
			_targetFilter = targetFilter;
		}
	}
}
