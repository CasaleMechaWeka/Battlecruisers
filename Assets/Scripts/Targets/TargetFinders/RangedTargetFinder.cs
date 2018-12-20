using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;

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

            _enemyDetector.OnEntered += OnEnemyEntered;
			_enemyDetector.OnExited += OnEnemyExited;

            _enemyDetector.StartDetecting();
		}

		private void OnEnemyEntered(object sender, TargetEventArgs args)
		{
			Logging.Log(Tags.TARGET_FINDER, "OnEnemyEntered()  " + args.Target);

            if (!args.Target.IsDestroyed
                && _targetFilter.IsMatch(args.Target) 
                && TargetFound != null)
			{
                Logging.Log(Tags.TARGET_FINDER, "Is Match!  " + args.Target);

				TargetFound.Invoke(this, args);
			}
		}

		private void OnEnemyExited(object sender, TargetEventArgs args)
		{
            Logging.Log(Tags.TARGET_FINDER, "OnEnemyExited()  " + args.Target);

			if (_targetFilter.IsMatch(args.Target) && TargetLost != null)
			{
                Logging.Log(Tags.TARGET_FINDER, "Is Match!  " + args.Target);
				
                TargetLost.Invoke(this, args);
			}
		}

		public void DisposeManagedState()
		{
			_enemyDetector.OnEntered -= OnEnemyEntered;
			_enemyDetector.OnExited -= OnEnemyExited;
		}
	}
}
