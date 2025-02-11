using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public interface IPvPCameraComponents
    {
        ICamera MainCamera { get; }
        IPvPCameraAdjuster CameraAdjuster { get; }
        IPvPCameraFocuser CameraFocuser { get; }
        IPvPCruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        Skybox Skybox { get; }
        IPvPCameraCalculatorSettings Settings { get; }
        IHighlightable NavigationButtonsPanel { get; }
    }
}