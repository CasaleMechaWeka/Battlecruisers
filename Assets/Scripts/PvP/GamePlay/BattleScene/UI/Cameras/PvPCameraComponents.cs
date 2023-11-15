using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public class PvPCameraComponents : IPvPCameraComponents
    {
        public IPvPCamera MainCamera { get; }
        public IPvPCameraAdjuster CameraAdjuster { get; }
        public IPvPCameraFocuser CameraFocuser { get; }
        public IPvPCruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        public Skybox Skybox { get; }
        public IPvPCameraCalculatorSettings Settings { get; }
        public IPvPHighlightable NavigationButtonsPanel { get; }

        public PvPCameraComponents(
            IPvPCamera mainCamera,
            IPvPCameraAdjuster cameraAdjuster,
            IPvPCameraFocuser cameraFocuser,
            IPvPCruiserDeathCameraFocuser cruiserDeathCameraFocuser,
            Skybox skybox,
            IPvPCameraCalculatorSettings settings,
            IPvPHighlightable navigationButtonsPanel)
        {
            PvPHelper.AssertIsNotNull(mainCamera, cameraAdjuster, cameraFocuser, cruiserDeathCameraFocuser, skybox, settings, navigationButtonsPanel);

            MainCamera = mainCamera;
            CameraAdjuster = cameraAdjuster;
            CameraFocuser = cameraFocuser;
            CruiserDeathCameraFocuser = cruiserDeathCameraFocuser;
            Skybox = skybox;
            Settings = settings;
            NavigationButtonsPanel = navigationButtonsPanel;
        }
    }
}