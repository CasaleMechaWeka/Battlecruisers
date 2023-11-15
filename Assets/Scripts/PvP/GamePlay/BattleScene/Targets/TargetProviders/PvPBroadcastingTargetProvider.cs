using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public abstract class PvPBroadcastingTargetProvider : IPvPBroadcastingTargetProvider
    {
        private IPvPTarget _target;
        public IPvPTarget Target
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
