using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.Targets.TargetProviders
{
    public abstract class BroadcastingTargetProvider : IBroadcastingTargetProvider
    {
        private ITarget _target;
        public ITarget Target 
        { 
            get { return _target; }
            protected set
            {
                if (_target != value)
                {
                    _target = value;

                    TargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler TargetChanged;

        public abstract void DisposeManagedState();
    }
}
