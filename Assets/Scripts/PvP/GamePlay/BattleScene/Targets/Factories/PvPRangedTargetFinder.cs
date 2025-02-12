using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders
{
    /// <summary>
    /// Target zone is a circle/rectangle.  Targets are found as they enter this circle/rectangle
    /// and lost as they exit.
    /// </summary>
    public class PvPRangedTargetFinder : ITargetFinder
    {
        private readonly ITargetDetector _enemyDetector;
        private readonly ITargetFilter _targetFilter;

        public event EventHandler<TargetEventArgs> TargetFound;
        public event EventHandler<TargetEventArgs> TargetLost;

        public PvPRangedTargetFinder(ITargetDetector enemyDetector, ITargetFilter targetFilter)
        {
            PvPHelper.AssertIsNotNull(enemyDetector, targetFilter);

            _enemyDetector = enemyDetector;
            _targetFilter = targetFilter;

            _enemyDetector.TargetEntered += OnEnemyEntered;
            _enemyDetector.TargetExited += OnEnemyExited;

            _enemyDetector.StartDetecting();
        }

        private void OnEnemyEntered(object sender, TargetEventArgs args)
        {
            // Logging.Verbose(Tags.TARGET_FINDER, args.Target.ToString());

            if (!args.Target.IsDestroyed
                && _targetFilter.IsMatch(args.Target))
            {
                // Logging.Verbose(Tags.TARGET_FINDER, "Is Match!  " + args.Target);

                TargetFound?.Invoke(this, args);
            }
        }

        private void OnEnemyExited(object sender, TargetEventArgs args)
        {
            // Logging.Verbose(Tags.TARGET_FINDER, args.Target.ToString());

            if (_targetFilter.IsMatch(args.Target))
            {
                // Logging.Verbose(Tags.TARGET_FINDER, "Is Match!  " + args.Target);

                TargetLost?.Invoke(this, args);
            }
        }

        public void DisposeManagedState()
        {
            _enemyDetector.TargetEntered -= OnEnemyEntered;
            _enemyDetector.TargetExited -= OnEnemyExited;
        }
    }
}
