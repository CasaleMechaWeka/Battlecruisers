using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
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
        ICameraCalculatorSettings Settings { get; }
        IHighlightable NavigationButtonsPanel { get; }
    }
}