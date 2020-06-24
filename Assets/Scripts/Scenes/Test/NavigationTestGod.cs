using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
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
            IBroadcastingFilter navigationWheelEnabledFilter = new StaticBroadcastingFilter(isMatch: true);

            _camera = new CameraBC(Camera.main);
            _cameraCalculatorSettings
                = new CameraCalculatorSettings(
                    Substitute.For<ISettingsManager>(),
                    _camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(_camera, _cameraCalculatorSettings);

            // FELIX  Create scrollwheel CTP instead?
            //// Instant, jerky adjuster
            //_cameraAdjuster = new InstantCameraAdjuster(cameraTargetProvider, _camera);

            //// Smooth adjuster
            //ITime time = TimeBC.Instance;
            //_cameraAdjuster
            //    = new SmoothCameraAdjuster(
            //        cameraTargetProvider,
            //        new SmoothZoomAdjuster(_camera, time, smoothTime),
            //        new SmoothPositionAdjuster(_camera.Transform, time, smoothTime));
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