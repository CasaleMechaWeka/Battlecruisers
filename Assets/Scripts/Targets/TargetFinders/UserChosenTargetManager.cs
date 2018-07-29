using BattleCruisers.Buildables;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
    /// <summary>
    /// The user can choose one target as the highest priority target,
    /// trumping other forms of target finding.  This class keeps track
    /// of that target.
    /// </summary>
    /// FELIX  Test, use
    public class UserChosenTargetManager : IUserChosenTargetManager
    {
        private ITarget _userChosenTarget;
        public ITarget UserChosenTarget
        {
            private get { return _userChosenTarget; }
            set
            {
                if (_userChosenTarget != null)
                {
                    if (TargetLost != null)
                    {
                        TargetLost.Invoke(this, new TargetEventArgs(_userChosenTarget));
                    }
                    _userChosenTarget.Destroyed -= _userChosenTarget_Destroyed;
                }

                _userChosenTarget = value;

                if (_userChosenTarget != null)
                {
                    if (TargetFound != null)
                    {
                        TargetFound.Invoke(this, new TargetEventArgs(_userChosenTarget));
                    }
                    _userChosenTarget.Destroyed += _userChosenTarget_Destroyed;
                }
            }
        }

        public event EventHandler<TargetEventArgs> TargetFound;
        public event EventHandler<TargetEventArgs> TargetLost;

        private void _userChosenTarget_Destroyed(object sender, DestroyedEventArgs e)
        {
            Assert.IsTrue(ReferenceEquals(UserChosenTarget, e.DestroyedTarget));
            UserChosenTarget = null;
        }

        public void StartFindingTargets()
        {
            // Empty
        }

        public void DisposeManagedState()
        {
            UserChosenTarget = null;
        }
    }
}