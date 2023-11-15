using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders
{
    /// <summary>
    /// Target zone is a donut.  Targets are found as they enter this donut 
    /// and lost as they exit.
    /// </summary>
    public class PvPMinRangeTargetFinder : IPvPTargetFinder
    {
        private readonly IPvPTargetDetector _maxRangeDetector, _minRangeDetector;
        private readonly IPvPTargetFilter _targetFilter;

        public event EventHandler<PvPTargetEventArgs> TargetFound;
        public event EventHandler<PvPTargetEventArgs> TargetLost;

        public PvPMinRangeTargetFinder(
            IPvPTargetDetector maxRangeDetector,
            IPvPTargetDetector minRangeDetector,
            IPvPTargetFilter targetFilter)
        {
            PvPHelper.AssertIsNotNull(maxRangeDetector, minRangeDetector, targetFilter);

            _maxRangeDetector = maxRangeDetector;
            _minRangeDetector = minRangeDetector;
            _targetFilter = targetFilter;

            _maxRangeDetector.TargetEntered += OnTargetFound;
            _maxRangeDetector.TargetExited += OnTargetLost;

            _minRangeDetector.TargetEntered += OnTargetLost;
            _minRangeDetector.TargetExited += OnTargetFound;

            _maxRangeDetector.StartDetecting();
            _minRangeDetector.StartDetecting();
        }

        private void OnTargetFound(object sender, PvPTargetEventArgs args)
        {
            //Debug.Log("Found");
            if (!args.Target.IsDestroyed
                && _targetFilter.IsMatch(args.Target))
            {
                TargetFound?.Invoke(this, args);
            }
        }

        private void OnTargetLost(object sender, PvPTargetEventArgs args)
        {
            //Debug.Log("Lost");
            if (_targetFilter.IsMatch(args.Target))
            {
                TargetLost?.Invoke(this, args);
            }
        }

        public void DisposeManagedState()
        {
            _maxRangeDetector.TargetEntered -= OnTargetFound;
            _maxRangeDetector.TargetExited -= OnTargetLost;

            _minRangeDetector.TargetEntered -= OnTargetLost;
            _minRangeDetector.TargetExited -= OnTargetFound;
        }
    }
}
