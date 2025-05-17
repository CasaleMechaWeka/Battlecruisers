using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class CameraComponents
    {
        public ICamera MainCamera { get; }
        public CameraAdjuster CameraAdjuster { get; }
        public ICameraFocuser CameraFocuser { get; }
        public ICruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        public Skybox Skybox { get; }
        public CameraCalculatorSettings Settings { get; }
        public IHighlightable NavigationButtonsPanel { get; }
        public IHighlightable CaptainsNavigationButtonsPanel { get; }

        public CameraComponents(
            ICamera mainCamera,
            CameraAdjuster cameraAdjuster,
            ICameraFocuser cameraFocuser,
            ICruiserDeathCameraFocuser cruiserDeathCameraFocuser,
            Skybox skybox,
            CameraCalculatorSettings settings,
            IHighlightable navigationButtonsPanel,
            IHighlightable captainsNavigationButtonsPanel)
        {
            Helper.AssertIsNotNull(mainCamera, cameraAdjuster, cameraFocuser, cruiserDeathCameraFocuser, skybox, settings, navigationButtonsPanel, captainsNavigationButtonsPanel);

            MainCamera = mainCamera;
            CameraAdjuster = cameraAdjuster;
            CameraFocuser = cameraFocuser;
            CruiserDeathCameraFocuser = cruiserDeathCameraFocuser;
            Skybox = skybox;
            Settings = settings;
            NavigationButtonsPanel = navigationButtonsPanel;
            CaptainsNavigationButtonsPanel = captainsNavigationButtonsPanel;
        }
    }
}