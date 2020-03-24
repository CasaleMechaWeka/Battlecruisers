using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Helpers.Pinch;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
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
        public TogglableDragTracker dragTracker;

        public ICameraComponents Initialise(
            ICamera camera, 
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser,
            NavigationPermitters navigationPermitters,
            ISwitchableUpdater switchableUpdater)
        {
            Helper.AssertIsNotNull(dragTracker, camera, settingsManager, playerCruiser, aiCruiser, navigationPermitters, switchableUpdater);

            switchableUpdater.Updated += SwitchableUpdater_Updated;

            dragTracker.Initialise(navigationPermitters.SwipeFilter);

            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationPermitters.NavigationWheelFilter);

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);
            IStaticCameraTargetProvider trumpCameraTargetProvider = new StaticCameraTargetProvider();

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
                    navigationPermitters,
                    trumpCameraTargetProvider);

            ITime time = TimeBC.Instance;

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, time, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, time, cameraSmoothTime));

            INavigationWheelPositionProvider navigationWheelPositionProvider 
                = new NavigationWheelPositionProvider(
                    navigationWheelPanel.PanelArea,
                    cameraNavigationWheelCalculator,
                    settings.ValidOrthographicSizes,
                    playerCruiser,
                    aiCruiser,
                    camera);

            Skybox skybox = GetComponent<Skybox>();
            Assert.IsNotNull(skybox);

            CameraFocuser cameraFocuser 
                = new CameraFocuser(
                    navigationWheelPositionProvider,
                    navigationWheelPanel.NavigationWheel,
                    trumpCameraTargetProvider);
            
            return
                new CameraComponents(
                    _cameraAdjuster,
                    navigationWheelPanel.NavigationWheel,
                    cameraFocuser,
                    new CruiserDeathCameraFocuser(cameraFocuser),
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
            NavigationPermitters navigationPermitters,
            IStaticCameraTargetProvider trumpCameraTargetProvider)
        {
            TogglableUpdater updater = GetComponent<TogglableUpdater>();
            Assert.IsNotNull(updater);
            updater.Initialise(navigationPermitters.ScrollWheelFilter);

            IPinchTracker pinchTracker = GetComponent<PinchTracker>();
            Assert.IsNotNull(pinchTracker);

            ICameraTargetFinder coreCameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetFinder cornerCameraTargetFinder
                = new NavigationWheelCornersCameraTargetFinder(
                    coreCameraTargetFinder,
                    new CornerIdentifier(
                        new CornerCutoffProvider(camera.Aspect)),
                    new CornerCameraTargetProvider(camera, cameraCalculator, settings, playerCruiser, aiCruiser));
            IUserInputCameraTargetProvider navigationWheelCameraTargetProvider 
                = new NavigationWheelCameraTargetProvider(
                    navigationWheelPanel.NavigationWheel, 
                    coreCameraTargetFinder,
                    cornerCameraTargetFinder);

            IUserInputCameraTargetProvider secondaryCameraTargetProvider 
                = CreateSecondaryCameraTargetProvider(
                    camera, 
                    cameraCalculator, 
                    settingsManager, 
                    settings, 
                    updater,
                    pinchTracker);


            // FELIX  Remove legacy :P
            IList<IUserInputCameraTargetProvider> cameraTargetProviders = new List<IUserInputCameraTargetProvider>()
            {
                navigationWheelCameraTargetProvider,
                secondaryCameraTargetProvider
            };

            return
                new CompositeCameraTargetProviderNEW(
                    cameraTargetProviders,
                    trumpCameraTargetProvider,
                    navigationWheelPanel.NavigationWheel,
                    cameraNavigationWheelCalculator);

            return
                new CompositeCameraTargetProvider(
                    navigationWheelCameraTargetProvider,
                    secondaryCameraTargetProvider,
                    trumpCameraTargetProvider,
                    navigationWheelPanel.NavigationWheel,
                    cameraNavigationWheelCalculator);
        }

        private IUserInputCameraTargetProvider CreateSecondaryCameraTargetProvider(
            ICamera camera, 
            ICameraCalculator cameraCalculator, 
            ISettingsManager settingsManager, 
            ICameraCalculatorSettings settings, 
            TogglableUpdater updater,
            IPinchTracker pinchTracker)
        {
            ISystemInfo systemInfo = new SystemInfoBC();
            IDirectionalZoom directionalZoom
                = new DirectionalZoom(
                    camera,
                    cameraCalculator,
                    settings.ValidOrthographicSizes);

            bool hasTouch = systemInfo.DeviceType == DeviceType.Handheld;
            // FELIX  TEMP
            //hasTouch = true;

            float zoomScale = hasTouch ? ZoomScale.SWIPE : ZoomScale.SCROLL_WHEEL;
            ZoomCalculator zoomCalculator 
                = new ZoomCalculator(
                    camera,
                    TimeBC.Instance,
                    settings.ValidOrthographicSizes,
                    settingsManager,
                    new ZoomLevelConverter(),
                    zoomScale);
            
            if (hasTouch)
            {
                // FELIX  TEMP
                return
                    new PinchZoomCameraTargetProvider(
                        zoomCalculator,
                        directionalZoom,
                        pinchTracker);

                return 
                    new SwipeCameraTargetProvider(
                        dragTracker,
                        new ScrollCalculator(
                            camera,
                            TimeBC.Instance,
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

        private void SwitchableUpdater_Updated(object sender, EventArgs e)
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}