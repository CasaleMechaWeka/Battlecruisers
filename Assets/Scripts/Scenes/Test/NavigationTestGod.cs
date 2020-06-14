using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : TestGodBase
    {
        private ICameraAdjuster _cameraAdjuster;
        protected ICamera _camera;
        protected ICameraCalculatorSettings _cameraCalculatorSettings;

        public float smoothTime = 0.15f;
        public bool useCorners = true;

        protected override void Setup(Helper helper)
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            IBroadcastingFilter navigationWheelEnabledFilter = new StaticBroadcastingFilter(isMatch: true);
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationWheelEnabledFilter);

            _camera = new CameraBC(Camera.main);
            _cameraCalculatorSettings
                = new CameraCalculatorSettings(
                    Substitute.For<ISettingsManager>(),
                    _camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(_camera, _cameraCalculatorSettings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator 
                = new CameraNavigationWheelCalculator(
                    navigationWheelPanel, 
                    cameraCalculator, 
                    _cameraCalculatorSettings.ValidOrthographicSizes,
                    new ProportionCalculator());
            ICameraTargetFinder cameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, _camera);

            ICameraTargetFinder cornersTargetFinder = cameraTargetFinder;
            if (useCorners)
            {
                ICruiser playerCruiser = CreateCruiser(isPlayerCruiser: true);
                ICruiser aiCruiser = CreateCruiser(isPlayerCruiser: false);

                cornersTargetFinder
                    = new NavigationWheelCornersCameraTargetFinder(
                        cameraTargetFinder,
                        new CornerIdentifier(
                            new CornerCutoffProvider(_camera.Aspect)),
                        new CornerCameraTargetProvider(_camera, cameraCalculator, _cameraCalculatorSettings, playerCruiser, aiCruiser));
            }

            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cameraTargetFinder, cornersTargetFinder);

            // Instant, jerky adjuster
            _cameraAdjuster = new InstantCameraAdjuster(cameraTargetProvider, _camera);

            // Smooth adjuster
            ITime time = TimeBC.Instance;
            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(_camera, time, smoothTime),
                    new SmoothPositionAdjuster(_camera.Transform, time, smoothTime));
        }

        private ICruiser CreateCruiser(bool isPlayerCruiser)
        {
            ICruiser cruiser = Substitute.For<ICruiser>();

            cruiser.Size.Returns(new Vector2(5, 5));
            float xPosition = isPlayerCruiser ? -35 : 35;
            cruiser.Position.Returns(new Vector2(xPosition, 0));
            cruiser.IsPlayerCruiser.Returns(isPlayerCruiser);

            return cruiser;
        }

        protected virtual void Update()
        {
            _cameraAdjuster?.AdjustCamera();
            //Debug.Log("Camera position: " + _camera.Transform.Position + "  Orthographic size: " + _camera.OrthographicSize);
        }
    }
}