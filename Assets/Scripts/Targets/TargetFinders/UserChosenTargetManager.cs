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
    public class UserChosenTargetManager : IUserChosenTargetManager
    {
        private ITarget _userChosenTarget;
        public ITarget Target
        {
            get { return _userChosenTarget; }
            set
            {
                if (ReferenceEquals(_userChosenTarget, value))
                {
                    return;
                }

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
            Assert.IsTrue(ReferenceEquals(Target, e.DestroyedTarget));
            Target = null;
        }

        public void StartFindingTargets()
        {
            // Empty
        }

        public void DisposeManagedState()
        {
            Target = null;
        }
    }
}