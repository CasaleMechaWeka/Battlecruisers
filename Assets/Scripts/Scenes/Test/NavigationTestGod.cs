using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets.Finders;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : MonoBehaviour
    {
        private ICameraAdjuster _cameraAdjuster;
        private ICamera _camera;

        public float smoothTime;
        public bool useCorners;

        protected virtual void Start()
        {
            NavigationWheelInitialiser navigationWheelInitialiser = FindObjectOfType<NavigationWheelInitialiser>();
            IBroadcastingFilter navigationWheelEnabledFilter = new StaticBroadcastingFilter(isMatch: true);
            INavigationWheelPanel navigationWheelPanel = navigationWheelInitialiser.InitialiseNavigationWheel(navigationWheelEnabledFilter);

            Camera platformCamera = FindObjectOfType<Camera>();
            _camera = new CameraBC(platformCamera);
            ICameraCalculatorSettings settings
                = new CameraCalculatorSettings(
                    Substitute.For<ISettingsManager>(),
                    _camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(_camera, settings);

            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator 
                = new CameraNavigationWheelCalculator(
                    navigationWheelPanel, 
                    cameraCalculator, 
                    settings.ValidOrthographicSizes,
                    new ProportionCalculator());
            ICameraTargetFinder cameraTargetFinder = new NavigationWheelCameraTargetFinder(cameraNavigationWheelCalculator, _camera);

            if (useCorners)
            {
                ICruiser playerCruiser = CreateCruiser(isPlayerCruiser: true);
                ICruiser aiCruiser = CreateCruiser(isPlayerCruiser: false);

                cameraTargetFinder
                    = new NavigationWheelCornersCameraTargetFinder(
                        cameraTargetFinder,
                        new CornerIdentifier(
                            new CornerCutoffProvider(_camera.Aspect)),
                        new CornerCameraTargetProvider(_camera, cameraCalculator, settings, playerCruiser, aiCruiser));
            }

            ICameraTargetProvider cameraTargetProvider = new NavigationWheelCameraTargetProvider(navigationWheelPanel.NavigationWheel, cameraTargetFinder);

            // Instant, jerky adjuster
            _cameraAdjuster = new InstantCameraAdjuster(cameraTargetProvider, _camera);

            // Smooth adjuster
            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(_camera, smoothTime),
                    new SmoothPositionAdjuster(_camera.Transform, smoothTime));
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

        private void Update()
        {
            _cameraAdjuster.AdjustCamera();
            //Debug.Log("Camera position: " + _camera.Transform.Position + "  Orthographic size: " + _camera.OrthographicSize);
        }
    }
}