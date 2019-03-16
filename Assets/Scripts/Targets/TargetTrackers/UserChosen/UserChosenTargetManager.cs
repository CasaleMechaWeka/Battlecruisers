using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetTrackers.UserChosen
{
    /// <summary>
    /// The user can choose one target as the highest priority target,
    /// trumping other forms of target finding.  This class keeps track
    /// of that target.
    /// </summary>
    public class UserChosenTargetManager : IUserChosenTargetManager
    {
        // The highest rank possible :)
        private const int USER_CHOSEN_TARGET_RANK = int.MaxValue;

        private ITarget _userChosenTarget;
        public ITarget Target
        {
            private get { return _userChosenTarget; }
            set
            {
                if (ReferenceEquals(_userChosenTarget, value))
                {
                    return;
                }

                if (_userChosenTarget != null)
                {
                    _userChosenTarget.Destroyed -= _userChosenTarget_Destroyed;
                }

                _userChosenTarget = value;
                HighestPriorityTarget = _userChosenTarget != null ? new RankedTarget(_userChosenTarget, USER_CHOSEN_TARGET_RANK) : null;

                if (_userChosenTarget != null)
                {
                    _userChosenTarget.Destroyed += _userChosenTarget_Destroyed;
                }

                HighestPriorityTargetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public RankedTarget HighestPriorityTarget { get; private set; }

        public event EventHandler HighestPriorityTargetChanged;

        private void _userChosenTarget_Destroyed(object sender, DestroyedEventArgs e)
        {
            Assert.IsTrue(ReferenceEquals(Target, e.DestroyedTarget));
            Target = null;
        }

        public void DisposeManagedState()
        {
            Target = null;
        }
    }
}