using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPManualProximityTargetDetector : IPvPManualProximityTargetDetector
    {
        private readonly IPvPTransform _parentTransform;
        private readonly IReadOnlyCollection<IPvPTarget> _potentialTargets;
        private readonly float _detectionRange;
        private readonly IPvPRangeCalculator _rangeCalculator;
        private readonly ISet<IPvPTarget> _currentInRangeTargets, _newInRangeTargets;
        private readonly IList<IPvPTarget> _exitedTargets;

        public event EventHandler<PvPTargetEventArgs> TargetEntered;
        public event EventHandler<PvPTargetEventArgs> TargetExited;

        public PvPManualProximityTargetDetector(
            IPvPTransform parentTransform,
            IReadOnlyCollection<IPvPTarget> potentialTargets,
            float detectionRange,
            IPvPRangeCalculator rangeCalculator)
        {
            PvPHelper.AssertIsNotNull(parentTransform, potentialTargets, rangeCalculator);

            _parentTransform = parentTransform;
            _potentialTargets = potentialTargets;
            _detectionRange = detectionRange;
            _rangeCalculator = rangeCalculator;

            _currentInRangeTargets = new HashSet<IPvPTarget>();
            _newInRangeTargets = new HashSet<IPvPTarget>();
            _exitedTargets = new List<IPvPTarget>();

            // Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"_potentialTargets.Count: {_potentialTargets.Count}");
        }

        public void Detect()
        {
            ISet<IPvPTarget> newInRangeTargets = FindInRangeTargets();
            // Logging.Verbose(Tags.MANUAL_TARGET_DETECTOR, $"Current targets: {_currentInRangeTargets.Count}  new targets: {newInRangeTargets.Count}");

            IList<IPvPTarget> exitedTargets = DetectExitedTargets(newInRangeTargets);
            foreach (IPvPTarget exitedTarget in exitedTargets)
            {
                // Logging.Log(Tags.MANUAL_TARGET_DETECTOR, $"Lost target: {exitedTarget}");

                _currentInRangeTargets.Remove(exitedTarget);
                TargetExited?.Invoke(this, new PvPTargetEventArgs(exitedTarget));
            }

            DetectEnteredTargets(newInRangeTargets);
        }

        private ISet<IPvPTarget> FindInRangeTargets()
        {
            // Logging.Verbose(Tags.MANUAL_TARGET_DETECTOR, $"_potentialTargets.Count: {_potentialTargets.Count}");

            _newInRangeTargets.Clear();

            foreach (IPvPTarget potentialTarget in _potentialTargets)
            {
                if (_rangeCalculator.IsInRange(_parentTransform, potentialTarget, _detectionRange))
                {
                    _newInRangeTargets.Add(potentialTarget);
                }
            }

            return _newInRangeTargets;
        }

        private IList<IPvPTarget> DetectExitedTargets(ISet<IPvPTarget> newInRangeTargets)
        {
            _exitedTargets.Clear();

            foreach (IPvPTarget currentInRangeTarget in _currentInRangeTargets)
            {
                if (!newInRangeTargets.Contains(currentInRangeTarget))
                {
                    _exitedTargets.Add(currentInRangeTarget);
                }
            }

            return _exitedTargets;
        }

        private void DetectEnteredTargets(ISet<IPvPTarget> newInRangeTargets)
        {
            foreach (IPvPTarget newInRangeTarget in newInRangeTargets)
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