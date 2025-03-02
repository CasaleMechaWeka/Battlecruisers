using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Pinch;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras
{
    public class PvPCameraInitialiser : MonoBehaviour
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

        public PvPTogglableDragTracker dragTracker;
        public Camera mainCamera;
        public Skybox skybox;
        public PvPNavigationButtonsPanel navigationButtonsPanel;
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
        private PvPCameraTransitionSpeedManager cameraTransitionSpeedManager;

        private PvPCruiser _playerCruiser;
        private PvPCruiser _enemyCruiser;

        public IPvPCameraComponents Initialise(
            ISettingsManager settingsManager,
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            NavigationPermitters navigationPermitters,
            ISwitchableUpdater switchableUpdater,
            ISingleSoundPlayer uiSoundPlayer,
            Team team)
        {
            PvPHelper.AssertIsNotNull(dragTracker, mainCamera, skybox, navigationButtonsPanel);
            PvPHelper.AssertIsNotNull(settingsManager, playerCruiser, enemyCruiser, navigationPermitters, switchableUpdater, uiSoundPlayer);

            switchableUpdater.Updated += SwitchableUpdater_Updated;

            icamera = new PvPCameraBC(mainCamera);
            dragTracker.Initialise(navigationPermitters.SwipeFilter);

            ICameraCalculatorSettings settings = new PvPCameraCalculatorSettings(settingsManager, icamera.Aspect);
            IPvPCameraCalculator cameraCalculator = new PvPCameraCalculator(icamera, settings);
            trumpCameraTargetProvider = new PvPStaticCameraTargetProvider(priority: 6);
            _playerCruiser = playerCruiser;
            _enemyCruiser = enemyCruiser;

            if (team == Team.LEFT)
            {
                targets
                    = new PvPCameraTargets(
                        cameraCalculator,
                        settings,
                        _playerCruiser,
                        _enemyCruiser,
                        icamera);
            }
            else
            {
                targets
                   = new PvPCameraTargets(
                    cameraCalculator,
                    settings,
                    _enemyCruiser,
                    _playerCruiser,
                    icamera);
            }

            defaultCameraTargetProvider = new PvPStaticCameraTargetProvider(priority: 1);
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

            time = PvPTimeBC.Instance;
            cameraTransitionSpeedManager = new PvPCameraTransitionSpeedManager(normalCameraSmoothTime, slowCameraSmoothTime);

            _cameraAdjuster
                = new PvPSmoothCameraAdjuster(
                    cameraTargetProvider,
                    new PvPSmoothZoomAdjuster(icamera, time, cameraTransitionSpeedManager),
                    new PvPSmoothPositionAdjuster(icamera, time, cameraTransitionSpeedManager));

            PvPCameraFocuser coreCameraFocuser
                = new PvPCameraFocuser(
                    targets,
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider,
                    cameraTransitionSpeedManager);

            cameraFocuser
                = new PvPIndirectCameraFocuser(
                    coreCameraFocuser,
                    icamera,
                    new PvPCameraTargetTracker(
                        icamera,
                        targets.OverviewTarget,
                        new PvPCameraTargetEqualityCalculator(
                            overviewPositionEqualityMarginInM,
                            overviewOrthographicSizeEqualityMargin)));

            navigationButtonsPanel.Initialise(navigationPermitters.NavigationButtonsFilter, cameraFocuser, uiSoundPlayer);
            //these must all be private variables.
            //cameraAdjuster must be reset using the new cameraTargetProvider
            //then camera components must be reassigned in battlescenegod
            return
                new PvPCameraComponents(
                    icamera,
                    _cameraAdjuster,
                    cameraFocuser,
                    new PvPCruiserDeathCameraFocuser(cameraFocuser),
                    skybox,
                    settings,
                    navigationButtonsPanel);
        }



        public IPvPCameraComponents Initialise(
            ISettingsManager settingsManager,
            IPvPCruiser playerCruiser,
            IPvPCruiser enemyCruiser,
            // PvPNavigationPermitters navigationPermitters,
            ISwitchableUpdater switchableUpdater,
            ISingleSoundPlayer uiSoundPlayer)
        {
            PvPHelper.AssertIsNotNull(dragTracker, mainCamera, skybox, navigationButtonsPanel);
            PvPHelper.AssertIsNotNull(settingsManager, playerCruiser, enemyCruiser, /* navigationPermitters, */ switchableUpdater, uiSoundPlayer);

            switchableUpdater.Updated += SwitchableUpdater_Updated;

            icamera = new PvPCameraBC(mainCamera);
            // dragTracker.Initialise(navigationPermitters.SwipeFilter);

            ICameraCalculatorSettings settings = new PvPCameraCalculatorSettings(settingsManager, icamera.Aspect);
            IPvPCameraCalculator cameraCalculator = new PvPCameraCalculator(icamera, settings);
            trumpCameraTargetProvider = new PvPStaticCameraTargetProvider(priority: 6);

            targets
                = new PvPCameraTargets(
                    cameraCalculator,
                    settings,
                    playerCruiser,
                    enemyCruiser,
                    icamera);

            defaultCameraTargetProvider = new PvPStaticCameraTargetProvider(priority: 1);
            defaultCameraTargetProvider.SetTarget(targets.PlayerCruiserTarget);

            ICameraTargetProvider cameraTargetProvider
                = CreateCameraTargetProvider(
                    icamera,
                    cameraCalculator,
                    settingsManager,
                    settings,
                    /*   navigationPermitters,*/
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider);

            time = PvPTimeBC.Instance;
            cameraTransitionSpeedManager = new PvPCameraTransitionSpeedManager(normalCameraSmoothTime, slowCameraSmoothTime);

            _cameraAdjuster
                = new PvPSmoothCameraAdjuster(
                    cameraTargetProvider,
                    new PvPSmoothZoomAdjuster(icamera, time, cameraTransitionSpeedManager),
                    new PvPSmoothPositionAdjuster(icamera, time, cameraTransitionSpeedManager));

            PvPCameraFocuser coreCameraFocuser
                = new PvPCameraFocuser(
                    targets,
                    trumpCameraTargetProvider,
                    defaultCameraTargetProvider,
                    cameraTransitionSpeedManager);

            cameraFocuser
                = new PvPIndirectCameraFocuser(
                    coreCameraFocuser,
                    icamera,
                    new PvPCameraTargetTracker(
                        icamera,
                        targets.OverviewTarget,
                        new PvPCameraTargetEqualityCalculator(
                            overviewPositionEqualityMarginInM,
                            overviewOrthographicSizeEqualityMargin)));

            // navigationButtonsPanel.Initialise(navigationPermitters.NavigationButtonsFilter, cameraFocuser, uiSoundPlayer);
            //these must all be private variables.
            //cameraAdjuster must be reset using the new cameraTargetProvider
            //then camera components must be reassigned in battlescenegod
            return
                new PvPCameraComponents(
                    icamera,
                    _cameraAdjuster,
                    cameraFocuser,
                    new PvPCruiserDeathCameraFocuser(cameraFocuser),
                    skybox,
                    settings,
                    navigationButtonsPanel);
        }
        private ICameraTargetProvider CreateCameraTargetProvider(
            ICamera camera,
            IPvPCameraCalculator cameraCalculator,
            ISettingsManager settingsManager,
            ICameraCalculatorSettings settings,
            NavigationPermitters navigationPermitters,
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider)
        {
            PvPTogglableUpdater updater = GetComponent<PvPTogglableUpdater>();
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
                new PvPCompositeCameraTargetProvider(
                    defaultCameraTargetProvider,
                    cameraTargetProviders);
        }


        private ICameraTargetProvider CreateCameraTargetProvider(
            ICamera camera,
            IPvPCameraCalculator cameraCalculator,
            ISettingsManager settingsManager,
            ICameraCalculatorSettings settings,
            // PvPNavigationPermitters navigationPermitters,
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider
        )
        {
            PvPTogglableUpdater updater = GetComponent<PvPTogglableUpdater>();
            Assert.IsNotNull(updater);
            // updater.Initialise(navigationPermitters.ScrollWheelAndPinchZoomFilter);

            IList<IUserInputCameraTargetProvider> cameraTargetProviders
                = CreateCameraTargetProviders(
                    camera,
                    cameraCalculator,
                    settingsManager,
                    settings,
                    updater,
                    trumpCameraTargetProvider);

            return
                new PvPCompositeCameraTargetProvider(
                    defaultCameraTargetProvider,
                    cameraTargetProviders);
        }

        private IList<IUserInputCameraTargetProvider> CreateCameraTargetProviders(
            ICamera camera,
            IPvPCameraCalculator cameraCalculator,
            ISettingsManager settingsManager,
            ICameraCalculatorSettings settings,
            PvPTogglableUpdater updater,
            IStaticCameraTargetProvider trumpCameraTargetProvider)
        {
            IPvPDirectionalZoom directionalZoom
                = new PvPDirectionalZoom(
                    camera,
                    cameraCalculator,
                    settings.ValidOrthographicSizes);

            IPvPInput input = PvPInputBC.Instance;
            IPvPPinchTracker pinchTracker = new PvPPinchTracker(input, updater);

            float zoomScale = PvPSystemInfoBC.Instance.IsHandheld ? PvPZoomScale.SWIPE : PvPZoomScale.SCROLL_WHEEL;
            float zoomSettingsMultiplier = new PvPZoomLevelConverter().LevelToMultiplier(settingsManager.ZoomSpeedLevel);

            PvPZoomCalculator zoomCalculator
                = new PvPZoomCalculator(
                    camera,
                    PvPTimeBC.Instance,
                    settings.ValidOrthographicSizes,
                    zoomScale,
                    zoomSettingsMultiplier);

            IList<IUserInputCameraTargetProvider> targetProviders = new List<IUserInputCameraTargetProvider>()
            {
                trumpCameraTargetProvider
            };

            IPvPScrollRecogniser scrollRecogniser;
            PvPScrollLevelConverter scrollLevelConverter = new PvPScrollLevelConverter();
            float swipeMultiplier = TOUCH_SWIPE_MULTIPLIER;

            if (PvPSystemInfoBC.Instance.IsHandheld)
            {
                scrollRecogniser = new PvPScrollRecogniser();

                targetProviders.Add(
                    new PvPPinchZoomCameraTargetProvider(
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
                scrollRecogniser = new PvPStaticScrollRecogniser(isScroll: true);

                targetProviders.Add(
                    new PvPScrollWheelCameraTargetProvider(
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
                    new PvPEdgeScrollingCameraTargetProvider(
                        updater,
                        new PvPEdgeScrollCalculator(
                            PvPTimeBC.Instance,
                            settingsManager,
                            scrollLevelConverter,
                            camera,
                            settings.ValidOrthographicSizes,
                            edgeScrollMultiplier),
                        camera,
                        cameraCalculator,
                        new PvPEdgeDetector(
                            input,
                            new PvPScreenBC(),
                            edgeRegionWidthInPixels),
                        new Clamper()));
            }

            targetProviders.Add(
                new PvPSwipeCameraTargetProvider(
                    dragTracker,
                    new PvPScrollCalculator(
                        camera,
                        PvPTimeBC.Instance,
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

        public PvPCameraComponents UpdateCamera(ISettingsManager settingsManager, NavigationPermitters navigationPermitters)
        {
            ICameraCalculatorSettings settings = new PvPCameraCalculatorSettings(settingsManager, icamera.Aspect);
            IPvPCameraCalculator cameraCalculator = new PvPCameraCalculator(icamera, settings);
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
                = new PvPSmoothCameraAdjuster(
                    cameraTargetProvider,
                    new PvPSmoothZoomAdjuster(icamera, time, cameraTransitionSpeedManager),
                    new PvPSmoothPositionAdjuster(icamera, time, cameraTransitionSpeedManager));

            return
                new PvPCameraComponents(
                    icamera,
                    _cameraAdjuster,
                    cameraFocuser,
                    new PvPCruiserDeathCameraFocuser(cameraFocuser),
                    skybox,
                    settings,
                    navigationButtonsPanel);
        }
    }
}