using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Helpers.Pinch;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using UnityCommon.PlatformAbstractions.Time;
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
        public Camera mainCamera;
        public Skybox skybox;
        public NavigationWheelInitialiser navigationWheelInitialiser;

        public ICameraComponents Initialise(
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser,
            NavigationPermitters navigationPermitters,
            ISwitchableUpdater switchableUpdater)
        {
            Helper.AssertIsNotNull(dragTracker, mainCamera, skybox, navigationWheelInitialiser);
            Helper.AssertIsNotNull(settingsManager, playerCruiser, aiCruiser, navigationPermitters, switchableUpdater);

            switchableUpdater.Updated += SwitchableUpdater_Updated;

            ICamera camera = new CameraBC(mainCamera);
            dragTracker.Initialise(navigationPermitters.SwipeFilter);

            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationPermitters.NavigationWheelFilter);

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(camera, settings);
            IStaticCameraTargetProvider trumpCameraTargetProvider = new StaticCameraTargetProvider(priority: 6);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator 
                = new CameraNavigationWheelCalculator(
                    navigationWheelPanel, 
                    cameraCalculator, 
                    settings.ValidOrthographicSizes,
                    new ProportionCalculator());

            ICameraTargets targets
                = new CameraTargets(
                    cameraCalculator,
                    settings,
                    playerCruiser,
                    aiCruiser,
                    camera);

            IStaticCameraTargetProvider defaultCameraTargetProvider = new StaticCameraTargetProvider(priority: 1);
            defaultCameraTargetProvider.SetTarget(targets.PlayerCruiserTarget);

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
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider);

            ITime time = TimeBC.Instance;

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(camera, time, cameraSmoothTime),
                    new SmoothPositionAdjuster(camera.Transform, time, cameraSmoothTime));

            CameraFocuser cameraFocuser 
                = new CameraFocuser(
                    targets,
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider);
            
            return
                new CameraComponents(
                    camera,
                    _cameraAdjuster,
                    navigationWheelPanel.NavigationWheel,
                    cameraFocuser,
                    new CruiserDeathCameraFocuser(cameraFocuser),
                    skybox,
                    settings);
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
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider)
        {
            TogglableUpdater updater = GetComponent<TogglableUpdater>();
            Assert.IsNotNull(updater);
            updater.Initialise(navigationPermitters.ScrollWheelAndPinchZoomFilter);

            ICameraTargetFinder coreCameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, camera);
            ICameraTargetFinder cornerCameraTargetFinder
                = new NavigationWheelCornersCameraTargetFinder(
                    coreCameraTargetFinder,
                    new CornerIdentifier(
                        new CornerCutoffProvider(camera.Aspect)),
                    new CornerCameraTargetProvider(camera, cameraCalculator, settings, playerCruiser, aiCruiser));
            // FELIX  Remove :)
            IUserInputCameraTargetProvider navigationWheelCameraTargetProvider 
                = new NavigationWheelCameraTargetProvider(
                    navigationWheelPanel.NavigationWheel, 
                    coreCameraTargetFinder,
                    cornerCameraTargetFinder);

            IList<IUserInputCameraTargetProvider> cameraTargetProviders 
                = CreateCameraTargetProviders(
                    camera, 
                    cameraCalculator, 
                    settingsManager, 
                    settings, 
                    updater,
                    trumpCameraTargetProvider);

            return
                new CompositeCameraTargetProvider(
                    defaultCameraTargetProvider,
                    cameraTargetProviders,
                    navigationWheelPanel.NavigationWheel,
                    cameraNavigationWheelCalculator);
        }

        private IList<IUserInputCameraTargetProvider> CreateCameraTargetProviders(
            ICamera camera,
            ICameraCalculator cameraCalculator,
            ISettingsManager settingsManager,
            ICameraCalculatorSettings settings,
            TogglableUpdater updater,
            IStaticCameraTargetProvider trumpCameraTargetProvider)
        {
            ISystemInfo systemInfo = new SystemInfoBC();
            IDirectionalZoom directionalZoom
                = new DirectionalZoom(
                    camera,
                    cameraCalculator,
                    settings.ValidOrthographicSizes);

            IInput input = new InputBC();
            IPinchTracker pinchTracker = new PinchTracker(input, updater);

            bool hasTouch = systemInfo.DeviceType == DeviceType.Handheld;

            float zoomScale = hasTouch ? ZoomScale.SWIPE : ZoomScale.SCROLL_WHEEL;
            ZoomCalculator zoomCalculator
                = new ZoomCalculator(
                    camera,
                    TimeBC.Instance,
                    settings.ValidOrthographicSizes,
                    settingsManager,
                    new ZoomLevelConverter(),
                    zoomScale);

            IList<IUserInputCameraTargetProvider> targetProviders = new List<IUserInputCameraTargetProvider>()
            {
                trumpCameraTargetProvider
            };

            if (hasTouch)
            {
                targetProviders.Add(
                    new PinchZoomCameraTargetProvider(
                        zoomCalculator,
                        directionalZoom,
                        pinchTracker));

                targetProviders.Add(
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
                        new BufferClamper(CAMERA_X_POSITION_BUFFER_IN_M)));
            }
            else
            {
                targetProviders.Add(
                    new ScrollWheelCameraTargetProvider(
                        input,
                        updater,
                        zoomCalculator,
                        directionalZoom));
            }

            return targetProviders;
        }

        private void SwitchableUpdater_Updated(object sender, EventArgs e)
        {
            _cameraAdjuster.AdjustCamera();
        }
    }
}