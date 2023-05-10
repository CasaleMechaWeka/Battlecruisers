using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public interface IPvPCameraTargetProvider
    {
        IPvPCameraTarget Target { get; }

        event EventHandler TargetChanged;
    }
}