using BattleCruisers.Projectiles.Trackers;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ITargetFilter _targetFilter;

        // Added trails to projectiles, making them visible even at low zoom.
        // Hence have rendered tracking obsolete :/  Keep in case I change my mind :D
        private const bool IS_TRACKING_ENABLED = false;

        public bool showTracker;

        public void Initialise(IProjectileSpawnerArgs args, ITargetFilter targetFilter)
		{
            base.Initialise(args);

            Assert.IsNotNull(targetFilter);
			_targetFilter = targetFilter;
		}

        protected void ShowTrackerIfNeeded(ITrackable shell)
        {
            if (IS_TRACKING_ENABLED && showTracker)
            {
                _factoryProvider.TrackerFactory.CreateTracker(shell);
            }
        }
	}
}
