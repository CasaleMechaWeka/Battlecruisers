using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
    public class CameraInitialiser : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;

        // Allows camera to be moved into invalid position up to this amount,
        // with camera snapping back into valid range when the navigation wheel
        // takes back over, which does not have this buffer.  Creates a nice
        // "springy" effect, instead of a hard stop of the swipe doing nothing.
        private const float CAMERA_X_POSITION_BUFFER_IN_M = 2;

        public float cameraSmoothTime;
        public DragTracker dragTracker;

        public ICameraComponents Initialise(
            ICamera camera, 
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser,
            IBroadcastingFilter navigationWheelEnabledFilter,
            IBroadcastingFilter scrollWheelEnabledFilter)
        {
            Helper.AssertIsNotNull(dragTracker, camera, settingsManager, playerCruiser, aiCruiser, navigationWheelEnabledFilter, scrollWheelEnabledFilter);

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
                    settingsManager,
                    settings,
                    navigationWheelPanel,
                    playerCruiser,
                    aiCruiser,
                    scrollWheelEnabledFilter);

            IDeltaTimeProvider deltaTimeProvider = new TimeBC();

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, deltaTimeProvider, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, deltaTimeProvider, cameraSmoothTime));

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
            ISettingsManager settingsManager,
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
            IUserInputCameraTargetProvider primaryCameraTargetProvider 
                = new NavigationWheelCameraTargetProvider(
                    navigationWheelPanel.NavigationWheel, 
                    coreCameraTargetFinder,
                    cornerCameraTargetFinder);

            IUserInputCameraTargetProvider secondaryCameraTargetProvider = CreateSecondaryCameraTargetProvider(camera, cameraCalculator, settingsManager, settings, updater);

            return
                new CompositeCameraTargetProvider(
                    primaryCameraTargetProvider,
                    secondaryCameraTargetProvider,
                    navigationWheelPanel.NavigationWheel,
                    cameraNavigationWheelCalculator);
        }

        private IUserInputCameraTargetProvider CreateSecondaryCameraTargetProvider(
            ICamera camera, 
            ICameraCalculator cameraCalculator, 
            ISettingsManager settingsManager, 
            ICameraCalculatorSettings settings, 
            TogglableUpdater updater)
        {
            ISystemInfo systemInfo = new SystemInfoBC();
            IDirectionalZoom directionalZoom
                = new DirectionalZoom(
                    camera,
                    cameraCalculator,
                    settings.ValidOrthographicSizes);

            bool hasTouch = systemInfo.DeviceType == DeviceType.Handheld;
            // FELIX  TEMP  For testing :P
            hasTouch = true;

            float zoomScale = hasTouch ? ZoomScale.SWIPE : ZoomScale.SCROLL_WHEEL;
            ZoomCalculator zoomCalculator 
                = new ZoomCalculator(
                    camera,
                    new TimeBC(),
                    settings.ValidOrthographicSizes,
                    settingsManager,
                    new ZoomLevelConverter(),
                    zoomScale);
            
            // FELIX  Update tutorial :)
            if (hasTouch)
            {
                return 
                    new SwipeCameraTargetProvider(
                        dragTracker,
                        new ScrollCalculator(
                            camera,
                            new TimeBC(),
                            settings.ValidOrthographicSizes,
                            settingsManager,
                            new ScrollLevelConverter()),
                        zoomCalculator,
                        camera,
                        cameraCalculator,
                        directionalZoom,
                        new ScrollRecogniser(),
                        new BufferClamper(CAMERA_X_POSITION_BUFFER_IN_M));
            }
            else
            {
                return
                    new ScrollWheelCameraTargetProvider(
                        new InputBC(),
                        updater,
                        zoomCalculator,
                        directionalZoom);
            }
        }

        public void Update()
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}