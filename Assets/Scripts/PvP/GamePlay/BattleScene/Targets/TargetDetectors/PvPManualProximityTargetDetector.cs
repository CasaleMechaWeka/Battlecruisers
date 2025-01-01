using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPManualProximityTargetDetector : IPvPManualProximityTargetDetector
    {
        private readonly ITransform _parentTransform;
        private readonly IReadOnlyCollection<ITarget> _potentialTargets;
        private readonly float _detectionRange;
        private readonly IPvPRangeCalculator _rangeCalculator;
        private readonly ISet<ITarget> _currentInRangeTargets, _newInRangeTargets;
        private readonly IList<ITarget> _exitedTargets;

        public event EventHandler<PvPTargetEventArgs> TargetEntered;
        public event EventHandler<PvPTargetEventArgs> TargetExited;

        public PvPManualProximityTargetDetector(
            ITransform parentTransform,
            IReadOnlyCollection<ITarget> potentialTargets,
            float detectionRange,
            IPvPRangeCalculator rangeCalculator)
        {
            PvPHelper.AssertIsNotNull(parentTransform, potentialTargets, rangeCalculator);

            _parentTransform = parentTransform;
            _potentialTargets = potentialTargets;
            _detectionRange = detectionRange;
            _rangeCalculator = rangeCalculator;

            _currentInRangeTargets = new HashSet<ITarget>();
            _newInRangeTargets = new HashSet<ITarget>();
            _exitedTargets = new List<ITarget>();

            // Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"_potentialTargets.Count: {_potentialTargets.Count}");
        }

        public void Detect()
        {
            ISet<ITarget> newInRangeTargets = FindInRangeTargets();
            // Logging.Verbose(Tags.MANUAL_TARGET_DETECTOR, $"Current targets: {_currentInRangeTargets.Count}  new targets: {newInRangeTargets.Count}");

            IList<ITarget> exitedTargets = DetectExitedTargets(newInRangeTargets);
            foreach (ITarget exitedTarget in exitedTargets)
            {
                // Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"Lost target: {exitedTarget}");

                _currentInRangeTargets.Remove(exitedTarget);
                TargetExited?.Invoke(this, new PvPTargetEventArgs(exitedTarget));
            }

            DetectEnteredTargets(newInRangeTargets);
        }

        private ISet<ITarget> FindInRangeTargets()
        {
            // Logging.Verbose(Tags.MANUAL_TARGET_DETECTOR, $"_potentialTargets.Count: {_potentialTargets.Count}");

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
                    // Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"Found new target: {newInRangeTarget}");

                    _currentInRangeTargets.Add(newInRangeTarget);
                    TargetEntered?.Invoke(this, new PvPTargetEventArgs(newInRangeTarget));
                }
            }
        }

        public void StartDetecting()
        {
            // empty
        }
    }
}