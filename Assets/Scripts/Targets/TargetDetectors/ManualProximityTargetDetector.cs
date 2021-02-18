using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Targets.TargetDetectors
{
    public class ManualProximityTargetDetector : IManualProximityTargetDetector
    {
        private readonly ITransform _parentTransform;
        private readonly IReadOnlyCollection<ITarget> _potentialTargets;
        private readonly float _detectionRange;
        private readonly IRangeCalculator _rangeCalculator;
        private readonly ISet<ITarget> _currentInRangeTargets, _newInRangeTargets;
        private readonly IList<ITarget> _exitedTargets;

        public event EventHandler<TargetEventArgs> TargetEntered;
        public event EventHandler<TargetEventArgs> TargetExited;

        public ManualProximityTargetDetector(
            ITransform parentTransform, 
            IReadOnlyCollection<ITarget> potentialTargets, 
            float detectionRange,
            IRangeCalculator rangeCalculator)
        {
            Helper.AssertIsNotNull(parentTransform, potentialTargets, rangeCalculator);

            _parentTransform = parentTransform;
            _potentialTargets = potentialTargets;
            _detectionRange = detectionRange;
            _rangeCalculator = rangeCalculator;

            _currentInRangeTargets = new HashSet<ITarget>();
            _newInRangeTargets = new HashSet<ITarget>();
            _exitedTargets = new List<ITarget>();
        }

        public void Detect()
        {
            ISet<ITarget> newInRangeTargets = FindInRangeTargets();
            Logging.Verbose(Tags.MANUAL_TARGET_DETECTOR, $"Current targets: {_currentInRangeTargets.Count}  new targets: {newInRangeTargets.Count}");

            IList<ITarget> exitedTargets = DetectExitedTargets(newInRangeTargets);
            foreach (ITarget exitedTarget in exitedTargets)
            {
                Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"Lost target: {exitedTarget}");

                _currentInRangeTargets.Remove(exitedTarget);
                TargetExited?.Invoke(this, new TargetEventArgs(exitedTarget));
            }

            DetectEnteredTargets(newInRangeTargets);
        }

        private ISet<ITarget> FindInRangeTargets()
        {
            _newInRangeTargets.Clear();

            foreach (ITarget potentialTarget in _potentialTargets)
            {
                if (_rangeCalculator.IsInRange(_parentTransform, potentialTarget, _detectionRange))
                {
                    _newInRangeTargets.Add(potentialTarget);
                }
            }

            return _newInRangeTargets;
        }

        private IList<ITarget> DetectExitedTargets(ISet<ITarget> newInRangeTargets)
        {
            _exitedTargets.Clear();
            
            foreach (ITarget currentInRangeTarget in _currentInRangeTargets)
            {
                if (!newInRangeTargets.Contains(currentInRangeTarget))
                {
                    _exitedTargets.Add(currentInRangeTarget);
                }
            }

            return _exitedTargets;
        }

        private void DetectEnteredTargets(ISet<ITarget> newInRangeTargets)
        {
            foreach (ITarget newInRangeTarget in newInRangeTargets)
            {
                if (!_currentInRangeTargets.Contains(newInRangeTarget))
                {
                    Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"Found new target: {newInRangeTarget}");

                    _currentInRangeTargets.Add(newInRangeTarget);
                    TargetEntered?.Invoke(this, new TargetEventArgs(newInRangeTarget));
                }
            }
        }

        public void StartDetecting()
        {
            // empty
        }
    }
}