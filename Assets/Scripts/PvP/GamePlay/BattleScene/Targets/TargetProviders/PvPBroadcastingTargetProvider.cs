using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetProviders;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public abstract class PvPBroadcastingTargetProvider : IBroadcastingTargetProvider
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
