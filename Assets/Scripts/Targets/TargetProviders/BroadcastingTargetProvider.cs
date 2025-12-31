using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProviders
{
    public abstract class BroadcastingTargetProvider : ITargetProvider, IManagedDisposable
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
