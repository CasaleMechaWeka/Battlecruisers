using BattleCruisers.UI.Cameras.Targets;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers
{
    public interface IPvPCameraTargetProvider
    {
        ICameraTarget Target { get; }

        event EventHandler TargetChanged;
    }
}