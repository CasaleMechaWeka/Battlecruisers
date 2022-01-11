using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Helpers.Pinch;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using BattleCruisers.Utils.PlatformAbstractions.Time;
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

        private const float TOUCH_SWIPE_MULTIPLIER = 16;
        private const float MOUSE_SWIPE_MULTIPLIER = 16;
        private const float EDGE_SCROLL_MULTIPLIER = 512;

        public TogglableDragTracker dragTracker;
        public Camera mainCamera;
        public Skybox skybox;
        public NavigationButtonsPanel navigationButtonsPanel;
        public float overviewPositionEqualityMarginInM = 2;
        public float overviewOrthographicSizeEqualityMargin = 2;
        public int edgeRegionWidthInPixels = 5;

        [Header("Smooth Time")]
        public float normalCameraSmoothTime = 0.15f;
        public float slowCameraSmoothTime = 0.5f;
        private ICamera icamera;
        private ICameraFocuser cameraFocuser;
        private IStaticCameraTargetProvider defaultCameraTargetProvider;
        private IStaticCameraTargetProvider trumpCameraTargetProvider;
        private ICameraTargets targets;
        private ITime time;
        private CameraTransitionSpeedManager cameraTransitionSpeedManager;

        public ICameraComponents Initialise(
            ISettingsManager settingsManager, 
            ICruiser playerCruiser, 
            ICruiser aiCruiser,
            NavigationPermitters navigationPermitters,
            ISwitchableUpdater switchableUpdater,
            ISingleSoundPlayer uiSoundPlayer)
        {
            Helper.AssertIsNotNull(dragTracker, mainCamera, skybox, navigationButtonsPanel);
            Helper.AssertIsNotNull(settingsManager, playerCruiser, aiCruiser, navigationPermitters, switchableUpdater, uiSoundPlayer);

            switchableUpdater.Updated += SwitchableUpdater_Updated;

            icamera = new CameraBC(mainCamera);
            dragTracker.Initialise(navigationPermitters.SwipeFilter);

            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, icamera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(icamera, settings);
            trumpCameraTargetProvider = new StaticCameraTargetProvider(priority: 6);

            targets
                = new CameraTargets(
                    cameraCalculator,
                    settings,
                    playerCruiser,
                    aiCruiser,
                    icamera);

            defaultCameraTargetProvider = new StaticCameraTargetProvider(priority: 1);
            defaultCameraTargetProvider.SetTarget(targets.PlayerCruiserTarget);

            ICameraTargetProvider cameraTargetProvider
                = CreateCameraTargetProvider(
                    icamera,
                    cameraCalculator,
                    settingsManager,
                    settings,
                    navigationPermitters,
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider);

            time = TimeBC.Instance;
            cameraTransitionSpeedManager = new CameraTransitionSpeedManager(normalCameraSmoothTime, slowCameraSmoothTime);

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(icamera, time, cameraTransitionSpeedManager),
                    new SmoothPositionAdjuster(icamera, time, cameraTransitionSpeedManager));

            CameraFocuser coreCameraFocuser 
                = new CameraFocuser(
                    targets,
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider,
                    cameraTransitionSpeedManager);

            cameraFocuser
                = new IndirectCameraFocuser(
                    coreCameraFocuser,
                    icamera,
                    new CameraTargetTracker(
                        icamera,
                        targets.OverviewTarget,
                        new CameraTargetEqualityCalculator(
                            overviewPositionEqualityMarginInM,
                            overviewOrthographicSizeEqualityMargin)));

            navigationButtonsPanel.Initialise(navigationPermitters.NavigationButtonsFilter, cameraFocuser, uiSoundPlayer);
            //these must all be private variables.
            //cameraAdjuster must be reset using the new cameraTargetProvider
            //then camera components must be reassigned in battlescenegod
            return
                new CameraComponents(
                    icamera,
                    _cameraAdjuster,
                    cameraFocuser,
                    new CruiserDeathCameraFocuser(cameraFocuser),
                    skybox,
                    settings,
                    navigationButtonsPanel);
        }

        private ICameraTargetProvider CreateCameraTargetProvider(
            ICamera camera,
            ICameraCalculator cameraCalculator,
            ISettingsManager settingsManager,
            ICameraCalculatorSettings settings,
            NavigationPermitters navigationPermitters,
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider)
        {
            TogglableUpdater updater = GetComponent<TogglableUpdater>();
            Assert.IsNotNull(updater);
            updater.Initialise(navigationPermitters.ScrollWheelAndPinchZoomFilter);

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
                    cameraTargetProviders);
        }

        private IList<IUserInputCameraTargetProvider> CreateCameraTargetProviders(
            ICamera camera,
            ICameraCalculator cameraCalculator,
            ISettingsManager settingsManager,
            ICameraCalculatorSettings settings,
            TogglableUpdater updater,
            IStaticCameraTargetProvider trumpCameraTargetProvider)
        {
            IDirectionalZoom directionalZoom
                = new DirectionalZoom(
                    camera,
                    cameraCalculator,
                    settings.ValidOrthographicSizes);

            IInput input = InputBC.Instance;
            IPinchTracker pinchTracker = new PinchTracker(input, updater);

            float zoomScale = SystemInfoBC.Instance.IsHandheld ? ZoomScale.SWIPE : ZoomScale.SCROLL_WHEEL;
            float zoomSettingsMultiplier = new ZoomLevelConverter().LevelToMultiplier(settingsManager.ZoomSpeedLevel);

            ZoomCalculator zoomCalculator
                = new ZoomCalculator(
                    camera,
                    TimeBC.Instance,
                    settings.ValidOrthographicSizes,
                    zoomScale,
                    zoomSettingsMultiplier);

            IList<IUserInputCameraTargetProvider> targetProviders = new List<IUserInputCameraTargetProvider>()
            {
                trumpCameraTargetProvider
            };

            IScrollRecogniser scrollRecogniser;
            ScrollLevelConverter scrollLevelConverter = new ScrollLevelConverter();
            float swipeMultiplier = TOUCH_SWIPE_MULTIPLIER;

            if (SystemInfoBC.Instance.IsHandheld)
            {
                scrollRecogniser = new ScrollRecogniser();

                targetProviders.Add(
                    new PinchZoomCameraTargetProvider(
                        zoomCalculator,
                        directionalZoom,
                        pinchTracker));
            }
            else
            {
                swipeMultiplier = MOUSE_SWIPE_MULTIPLIER;
                if (Application.isEditor)
                {
                    // For some reason the editor scrolls slower than a Windows build :/
                    swipeMultiplier *= 4;
                }

                // Always interpret as scroll (swipe), never zoom.  Zoom is handled by scroll wheel :)
                scrollRecogniser = new StaticScrollRecogniser(isScroll: true);

                targetProviders.Add(
                    new ScrollWheelCameraTargetProvider(
                        input,
                        updater,
                        zoomCalculator,
                        directionalZoom));

                float edgeScrollMultiplier = EDGE_SCROLL_MULTIPLIER;
                if (Application.isEditor)
                {
                    // For some reason the editor scrolls slower than a Windows build :/
                    edgeScrollMultiplier *= 4;
                }

                targetProviders.Add(
                    new EdgeScrollingCameraTargetProvider(
                        updater,
                        new EdgeScrollCalculator(
                            TimeBC.Instance,
                            settingsManager,
                            scrollLevelConverter,
                            camera,
                            settings.ValidOrthographicSizes,
                            edgeScrollMultiplier),
                        camera,
                        cameraCalculator,
                        new EdgeDetector(
                            input,
                            new ScreenBC(),
                            edgeRegionWidthInPixels),
                        new Clamper()));
            }

            targetProviders.Add(
                new SwipeCameraTargetProvider(
                    dragTracker,
                    new ScrollCalculator(
                        camera,
                        TimeBC.Instance,
                        settings.ValidOrthographicSizes,
                        settingsManager,
                        scrollLevelConverter,
                        swipeMultiplier),
                    zoomCalculator,
                    camera,
                    cameraCalculator,
                    directionalZoom,
                    scrollRecogniser,
                    new BufferClamper(CAMERA_X_POSITION_BUFFER_IN_M)));

            return targetProviders;
        }

        private void SwitchableUpdater_Updated(object sender, EventArgs e)
        {
            _cameraAdjuster.AdjustCamera();
        }

        public CameraComponents UpdateCamera(ISettingsManager settingsManager, NavigationPermitters navigationPermitters)
        {
            ICameraCalculatorSettings settings = new CameraCalculatorSettings(settingsManager, icamera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(icamera, settings);
            ICameraTargetProvider cameraTargetProvider
                = CreateCameraTargetProvider(
                    icamera,
                    cameraCalculator,
                    settingsManager,
                    settings,
                    navigationPermitters,
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider);

            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(icamera, time, cameraTransitionSpeedManager),
                    new SmoothPositionAdjuster(icamera, time, cameraTransitionSpeedManager));

            return
                new CameraComponents(
                    icamera,
                    _cameraAdjuster,
                    cameraFocuser,
                    new CruiserDeathCameraFocuser(cameraFocuser),
                    skybox,
                    settings,
                    navigationButtonsPanel);
        }
    }
}