using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPTargetEventArgs : EventArgs
    {
        public PvPTargetEventArgs(IPvPTarget target)
        {
            Target = target;
        }

        public IPvPTarget Target { get; }
    }

    public interface IPvPTargetDetector
    {
        void StartDetecting();

        event EventHandler<PvPTargetEventArgs> TargetEntered;
        event EventHandler<PvPTargetEventArgs> TargetExited;
    }
}

