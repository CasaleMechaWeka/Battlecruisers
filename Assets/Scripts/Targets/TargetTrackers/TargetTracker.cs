using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetTrackers
{
    /// <summary>
    /// Keeps track of all targets found by the given ITargetFinder.
    /// </summary>
    public class TargetTracker : ITargetTracker
	{
        private readonly ITargetFinder _targetFinder;
        private readonly IList<ITarget> _targets;

		public event EventHandler TargetsChanged;

        public TargetTracker(ITargetFinder targetFinder)
        {
            Assert.IsNotNull(targetFinder);

            _targetFinder = targetFinder;
            _targets = new List<ITarget>();

            _targetFinder.TargetFound += _targetFinder_TargetFound;
            _targetFinder.TargetLost += _targetFinder_TargetLost;
        }

        private void _targetFinder_TargetFound(object sender, TargetEventArgs e)
        {
            Logging.Log(Tags.TARGET_TRACKER, e.Target.ToString());

            // Should always be the case but defensive programming because rarely it is 
            // NOT the case :/
            if (!_targets.Contains(e.Target))
            {
                _targets.Add(e.Target);
                EmitTargetsChangedEvent();
			}
        }

        private void _targetFinder_TargetLost(object sender, TargetEventArgs e)
        {
            Logging.Log(Tags.TARGET_TRACKER, e.Target.ToString());

            // Should always be the case but defensive programming because rarely it is 
            // NOT the case :/
			if (_targets.Contains(e.Target))
            {
                _targets.Remove(e.Target);
                EmitTargetsChangedEvent();
			}
        }

        private void EmitTargetsChangedEvent()
        {
            TargetsChanged?.Invoke(this, EventArgs.Empty);
        }

        // PERF  Hashset instead of list?
        public bool ContainsTarget(ITarget target)
        {
            bool result =_targets.Contains(target);
            Logging.Log(Tags.TARGET_TRACKER, $"result: {result}  _targets.Count: {_targets.Count}");
            return result;
        }

        public void DisposeManagedState()
        {
            _targetFinder.TargetFound -= _targetFinder_TargetFound;
            _targetFinder.TargetLost -= _targetFinder_TargetLost;
            _targets.Clear();
            EmitTargetsChangedEvent();
        }
    }
}
