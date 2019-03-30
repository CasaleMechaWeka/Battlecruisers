using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
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
            IBroadcastingFilter navigationWheelEnabledFilter,
            IBroadcastingFilter scrollWheelEnabledFilter)
        {
            Helper.AssertIsNotNull(camera, settingsManager, playerCruiser, aiCruiser, navigationWheelEnabledFilter, scrollWheelEnabledFilter);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationWheelEnabledFilter);

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator 
                = new CameraNavigationWheelCalculator(
                    navigationWheelPanel, 
                    cameraCalculator, 
                    settings.ValidOrthographicSizes,
                    new ProportionCalculator());

            ICameraTargetProvider cameraTargetProvider
                = CreateCameraTargetProvider(
                    camera,
                    cameraCalculator,
                    cameraNavigationWheelCalculator,
                    settings,
                    navigationWheelPanel,
                    playerCruiser,
                    aiCruiser,
                    scrollWheelEnabledFilter);

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, cameraSmoothTime));

            INavigationWheelPositionProvider navigationWheelPositionProvider 
                = new NavigationWheelPositionProvider(
                    navigationWheelPanel.PanelArea,
                    cameraNavigationWheelCalculator,
                    settings.ValidOrthographicSizes,
                    playerCruiser,
                    aiCruiser);

            Skybox skybox = GetComponent<Skybox>();
            Assert.IsNotNull(skybox);

            return
                new CameraComponents(
                    _cameraAdjuster,
                    navigationWheelPanel.NavigationWheel,
                    new CameraFocuser(navigationWheelPositionProvider, navigationWheelPanel.NavigationWheel),
                    skybox);
        }

        private ICameraTargetProvider CreateCameraTargetProvider(
            ICamera camera,
            ICameraCalculator cameraCalculator,
            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator,
            ICameraCalculatorSettings settings,
            INavigationWheelPanel navigationWheelPanel,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBroadcastingFilter scrollWheelEnabledFilter)
        {
            TogglableUpdater updater = GetComponent<TogglableUpdater>();
            Assert.IsNotNull(updater);
            updater.Initialise(scrollWheelEnabledFilter);

            ICameraTargetFinder coreCameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetFinder cornerCameraTargetFinder
                = new NavigationWheelCornersCameraTargetFinder(
                    coreCameraTargetFinder,
                    new CornerIdentifier(
                        new CornerCutoffProvider(camera.Aspect)),
                    new CornerCameraTargetProvider(camera, cameraCalculator, settings, playerCruiser, aiCruiser));
            ICameraTargetProvider navigationWheelCameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cornerCameraTargetFinder);

            ISystemInfo systemInfo = new SystemInfoBC();

            if (systemInfo.DeviceType == DeviceType.Handheld)
            {
                // Handhelds have no mouse or scroll wheel, so ignore that navigation method
                return navigationWheelCameraTargetProvider;
            }

            IScrollWheelCameraTargetProvider scrollWheelCameraTargetProvider
                = new ScrollWheelCameraTargetProvider(
                    camera,
                    cameraCalculator,
                    new InputBC(),
                    settings.ValidOrthographicSizes,
                    updater,
                    new ZoomCalculator(
                        camera,
                        new TimeBC(),
                        settings.ValidOrthographicSizes));

            return
                new CompositeCameraTargetProvider(
                    navigationWheelCameraTargetProvider,
                    scrollWheelCameraTargetProvider,
                    navigationWheelPanel.NavigationWheel,
                    cameraNavigationWheelCalculator);
        }

        public void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}