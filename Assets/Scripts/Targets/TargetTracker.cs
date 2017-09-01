using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets
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
            Assert.IsFalse(_targets.Contains(e.Target));
            _targets.Add(e.Target);
            EmitTargetsChangedEvent();
        }

        private void _targetFinder_TargetLost(object sender, TargetEventArgs e)
        {
            Assert.IsTrue(_targets.Contains(e.Target));
            _targets.Remove(e.Target);
            EmitTargetsChangedEvent();
        }

        private void EmitTargetsChangedEvent()
        {
            if (TargetsChanged != null)
            {
                TargetsChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public bool ContainsTarget(ITarget target)
        {
            return _targets.Contains(target);
        }

        public void Dispose()
        {
            _targetFinder.TargetFound -= _targetFinder_TargetFound;
            _targetFinder.TargetLost -= _targetFinder_TargetLost;
        }
    }
}
