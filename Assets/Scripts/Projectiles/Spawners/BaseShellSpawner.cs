using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ITargetFilter _targetFilter;

        public bool showTracker;

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter)
		{
            base.Initialise(args);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}

        protected void ShowTrackerIfNeeded(ITrackable shell)
        {
            if (showTracker)
            {
                _factoryProvider.TrackerFactory.CreateTracker(shell);
            }
        }
	}
}
