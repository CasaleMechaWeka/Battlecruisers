using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Targets.TargetDetectors
{
    // FELIX  Test
    // FELIX  Use
    public class ProximityTargetDetector : ITargetDetector, IManualDetector
    {
        private readonly ITransform _parentTransform;
        private readonly IUnitProvider _potentialTargets;
        private readonly float _detectionRange;
        private readonly ISet<ITarget> _currentInRangeTargets, _newInRangeTargets;

        public event EventHandler<TargetEventArgs> TargetEntered;
        public event EventHandler<TargetEventArgs> TargetExited;

        public ProximityTargetDetector(ITransform parentTransform, IUnitProvider potentialTargets, float detectionRange)
        {
            Helper.AssertIsNotNull(parentTransform, potentialTargets);

            _parentTransform = parentTransform;
            _potentialTargets = potentialTargets;
            _detectionRange = detectionRange;
            _currentInRangeTargets = new HashSet<ITarget>();
            _newInRangeTargets = new HashSet<ITarget>();
        }

        public void Detect()
        {
            ISet<ITarget> newInRangeTargets = FindInRangeTargets();
            Logging.Verbose(Tags.TARGET_DETECTOR, $"Current targets: {_currentInRangeTargets.Count}  new targets: {newInRangeTargets.Count}");

            // Find targets that have left
            foreach (ITarget currentInRangeTarget in _currentInRangeTargets)
            {
                if (!newInRangeTargets.Contains(currentInRangeTarget))
                {
                    Logging.Log(Tags.TARGET_DETECTOR, $"Lost target: {currentInRangeTarget}");

                    _currentInRangeTargets.Remove(currentInRangeTarget);
                    TargetExited?.Invoke(this, new TargetEventArgs(currentInRangeTarget));
                }
            }

            // Find targets that are new
            foreach (ITarget newInRangeTarget in newInRangeTargets)
            {
                if (!_currentInRangeTargets.Contains(newInRangeTarget))
                {
                    Logging.Log(Tags.TARGET_DETECTOR, $"Found new target: {newInRangeTarget}");

                    _currentInRangeTargets.Add(newInRangeTarget);
                    TargetEntered?.Invoke(this, new TargetEventArgs(newInRangeTarget));
                }
            }
        }

        private ISet<ITarget> FindInRangeTargets()
        {
            _newInRangeTargets.Clear();

            foreach (IUnit potentialTarget in _potentialTargets.AliveUnits)
            {
                if (Vector2.Distance(potentialTarget.Transform.Position, _parentTransform.Position) <= _detectionRange)
                {
                    _newInRangeTargets.Add(potentialTarget);
                }
            }

            return _newInRangeTargets;
        }

        public void StartDetecting()
        {
            // empty
        }
    }
}