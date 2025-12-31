using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public class PvPCameraComponents
    {
        public ICamera MainCamera { get; }
        public CameraAdjuster CameraAdjuster { get; }
        public ICameraFocuser CameraFocuser { get; }
        public PvPCruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        public Skybox Skybox { get; }
        public CameraCalculatorSettings Settings { get; }
        public IHighlightable NavigationButtonsPanel { get; }

        public PvPCameraComponents(
            ICamera mainCamera,
            CameraAdjuster cameraAdjuster,
            ICameraFocuser cameraFocuser,
            PvPCruiserDeathCameraFocuser cruiserDeathCameraFocuser,
            Skybox skybox,
            CameraCalculatorSettings settings,
            IHighlightable navigationButtonsPanel)
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