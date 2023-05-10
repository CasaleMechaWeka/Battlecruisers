using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public interface IPvPCameraAdjuster
    {
        event EventHandler CompletedAdjustment;

        void AdjustCamera();
    }
}