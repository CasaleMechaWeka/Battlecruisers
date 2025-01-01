using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors
{
    public class PvPTargetEventArgs : EventArgs
    {
        public PvPTargetEventArgs(ITarget target)
        {
            Target = target;
        }

        public ITarget Target { get; }
    }

    public interface IPvPTargetDetector
    {
        void StartDetecting();

        event EventHandler<PvPTargetEventArgs> TargetEntered;
        event EventHandler<PvPTargetEventArgs> TargetExited;
    }
}

