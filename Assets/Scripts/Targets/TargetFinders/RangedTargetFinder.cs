using System;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetFinders
{
    /// <summary>
    /// Target zone is a circle.  Targets are found as they enter this circle
    /// and lost as they exit.
    /// </summary>
    public class RangedTargetFinder : ITargetFinder
	{
		private readonly ITargetDetector _enemyDetector;
		private readonly ITargetFilter _targetFilter;

		public event EventHandler<TargetEventArgs> TargetFound;
		public event EventHandler<TargetEventArgs> TargetLost;

		public RangedTargetFinder(ITargetDetector enemyDetector, ITargetFilter targetFilter)
		{
            Helper.AssertIsNotNull(enemyDetector, targetFilter);

			_enemyDetector = enemyDetector;
			_targetFilter = targetFilter;
		}

		public void StartFindingTargets()
		{
			_enemyDetector.OnEntered += OnEnemyEntered;
			_enemyDetector.OnExited += OnEnemyExited;
		}

		private void OnEnemyEntered(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.TARGET_FINDER, "OnEnemyEntered()");

			if (_targetFilter.IsMatch(args.Target) && TargetFound != null)
			{
				TargetFound.Invoke(this, args);
			}
		}

		private void OnEnemyExited(object sender, TargetEventArgs args)
		{
            Logging.Log(Tags.TARGET_FINDER, "OnEnemyExited()");

			if (_targetFilter.IsMatch(args.Target) && TargetLost != null)
			{
				TargetLost.Invoke(this, args);
			}
		}

		public void Dispose()
		{
			_enemyDetector.OnEntered -= OnEnemyEntered;
			_enemyDetector.OnExited -= OnEnemyExited;
		}
	}
}
