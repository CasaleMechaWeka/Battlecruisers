using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders
{
    /// <summary>
    /// Target zone is a circle/rectangle.  Targets are found as they enter this circle/rectangle
    /// and lost as they exit.
    /// </summary>
    public class PvPRangedTargetFinder : IPvPTargetFinder
    {
        private readonly IPvPTargetDetector _enemyDetector;
        private readonly IPvPTargetFilter _targetFilter;

        public event EventHandler<PvPTargetEventArgs> TargetFound;
        public event EventHandler<PvPTargetEventArgs> TargetLost;

        public PvPRangedTargetFinder(IPvPTargetDetector enemyDetector, IPvPTargetFilter targetFilter)
        {
            PvPHelper.AssertIsNotNull(enemyDetector, targetFilter);

            _enemyDetector = enemyDetector;
            _targetFilter = targetFilter;

            _enemyDetector.TargetEntered += OnEnemyEntered;
            _enemyDetector.TargetExited += OnEnemyExited;

            _enemyDetector.StartDetecting();
        }

        private void OnEnemyEntered(object sender, PvPTargetEventArgs args)
        {
            // Logging.Verbose(Tags.TARGET_FINDER, args.Target.ToString());

            if (!args.Target.IsDestroyed
                && _targetFilter.IsMatch(args.Target))
            {
                // Logging.Verbose(Tags.TARGET_FINDER, "Is Match!  " + args.Target);

                TargetFound?.Invoke(this, args);
            }
        }

        private void OnEnemyExited(object sender, PvPTargetEventArgs args)
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
