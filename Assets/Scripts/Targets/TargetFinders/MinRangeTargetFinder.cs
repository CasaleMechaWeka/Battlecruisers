using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetFinders
{
    /// <summary>
    /// Target zone is a donut.  Targets are found as they enter this donut 
    /// and lost as they exit.
    /// </summary>
    public class MinRangeTargetFinder : ITargetFinder
    {
        private readonly ITargetDetector _maxRangeDetector, _minRangeDetector;
        private readonly ITargetFilter _targetFilter;

        public event EventHandler<TargetEventArgs> TargetFound;
        public event EventHandler<TargetEventArgs> TargetLost;

        public MinRangeTargetFinder(
            ITargetDetector maxRangeDetector, 
            ITargetDetector minRangeDetector, 
            ITargetFilter targetFilter)
        {
            Helper.AssertIsNotNull(maxRangeDetector, minRangeDetector, targetFilter);

            _maxRangeDetector = maxRangeDetector;
            _minRangeDetector = minRangeDetector;
            _targetFilter = targetFilter;

            _maxRangeDetector.OnEntered += OnTargetFound;
            _maxRangeDetector.OnExited += OnTargetLost;

            _minRangeDetector.OnEntered += OnTargetLost;
            _minRangeDetector.OnExited += OnTargetFound;

            _maxRangeDetector.StartDetecting();
            _minRangeDetector.StartDetecting();
        }

        private void OnTargetFound(object sender, TargetEventArgs args)
        {
            if (!args.Target.IsDestroyed
                && _targetFilter.IsMatch(args.Target) 
                && TargetFound != null)
            {
                TargetFound.Invoke(this, args);
            }
        }

        private void OnTargetLost(object sender, TargetEventArgs args)
        {
            if (_targetFilter.IsMatch(args.Target) 
                && TargetLost != null)
            {
                TargetLost.Invoke(this, args);
            }
        }

        public void DisposeManagedState()
        {
            _maxRangeDetector.OnEntered -= OnTargetFound;
            _maxRangeDetector.OnExited -= OnTargetLost;

            _minRangeDetector.OnEntered -= OnTargetLost;
            _minRangeDetector.OnExited -= OnTargetFound;
        }
    }
}
