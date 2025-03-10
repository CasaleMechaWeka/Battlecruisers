using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    public class UserChosenInRangeTargetTracker : IRankedTargetTracker
    {
        private readonly ITargetTracker _inRangeTargetTracker;
        private readonly IRankedTargetTracker _userChosenTargetTracker;

        private RankedTarget _highestPriorityTarget;
        public RankedTarget HighestPriorityTarget
        {
            get { return _highestPriorityTarget; }
            private set
            {
                if (!ReferenceEquals(_highestPriorityTarget, value))
                {
                    _highestPriorityTarget = value;

                    HighestPriorityTargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler HighestPriorityTargetChanged;

        public UserChosenInRangeTargetTracker(ITargetTracker inRangeTargetTracker, IRankedTargetTracker userChosenTargetTracker)
        {
            Helper.AssertIsNotNull(inRangeTargetTracker, userChosenTargetTracker);

            _inRangeTargetTracker = inRangeTargetTracker;
            _userChosenTargetTracker = userChosenTargetTracker;

            _inRangeTargetTracker.TargetsChanged += _inRangeTargetTracker_TargetsChanged;
            _userChosenTargetTracker.HighestPriorityTargetChanged += _userChosenTargetTracker_HighestPriorityTargetChanged;

            HighestPriorityTarget = FindHighestPriorityTarget();
        }

        private void _inRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            HighestPriorityTarget = FindHighestPriorityTarget();
        }

        private void _userChosenTargetTracker_HighestPriorityTargetChanged(object sender, EventArgs e)
        {
            HighestPriorityTarget = FindHighestPriorityTarget();
        }

        private RankedTarget FindHighestPriorityTarget()
        {
            RankedTarget highestRankedTarget = null;

            if (_userChosenTargetTracker.HighestPriorityTarget != null
                && _inRangeTargetTracker.ContainsTarget(_userChosenTargetTracker.HighestPriorityTarget.Target))
            {
                highestRankedTarget = _userChosenTargetTracker.HighestPriorityTarget;
            }

            return highestRankedTarget;
        }

        public void DisposeManagedState()
        {
            _inRangeTargetTracker.TargetsChanged -= _inRangeTargetTracker_TargetsChanged;
            _inRangeTargetTracker.DisposeManagedState();

            _userChosenTargetTracker.HighestPriorityTargetChanged -= _userChosenTargetTracker_HighestPriorityTargetChanged;
            // Do not dispose because shared by everyone, not just this buildable
        }
    }
}