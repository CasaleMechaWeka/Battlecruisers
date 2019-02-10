using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
    public class CameraInitialiser : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;

        public float cameraSmoothTime;

        public ICameraComponents Initialise(
            ICamera camera, 
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser,
            IBroadcastingFilter navigationWheelEnabledFilter)
        {
            Helper.AssertIsNotNull(camera, settingsManager, playerCruiser, aiCruiser, navigationWheelEnabledFilter);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationWheelEnabledFilter);

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(navigationWheelPanel, cameraCalculator, settings.ValidOrthographicSizes);
            ICameraTargetFinder coreCameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetFinder cornerCameraTargetFinder
                = new NavigationWheelCornersCameraTargetFinder(
                    coreCameraTargetFinder,
                    new CornerIdentifier(
                        new CornerCutoffProvider(camera.Aspect)),
                    new CornerCameraTargetProvider(camera, cameraCalculator, settings, playerCruiser, aiCruiser));
            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cornerCameraTargetFinder);

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, cameraSmoothTime));

            Skybox skybox = GetComponent<Skybox>();
            Assert.IsNotNull(skybox);

            INavigationWheelPositionProvider navigationWheelPositionProvider 
                = new NavigationWheelPositionProvider(
                    navigationWheelPanel.PanelArea,
                    cameraNavigationWheelCalculator,
                    settings.ValidOrthographicSizes,
                    playerCruiser,
                    aiCruiser);

            return
                new CameraComponents(
                    _cameraAdjuster,
                    navigationWheelPanel.NavigationWheel,
                    new CameraFocuser(navigationWheelPositionProvider, navigationWheelPanel.NavigationWheel),
                    skybox);
        }

        public void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}