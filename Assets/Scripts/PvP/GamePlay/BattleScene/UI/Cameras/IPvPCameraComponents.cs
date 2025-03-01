using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public interface IPvPCameraComponents
    {
        ICamera MainCamera { get; }
        ICameraAdjuster CameraAdjuster { get; }
        ICameraFocuser CameraFocuser { get; }
        IPvPCruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        Skybox Skybox { get; }
        ICameraCalculatorSettings Settings { get; }
        IHighlightable NavigationButtonsPanel { get; }
    }
}