using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using System.Collections.Generic;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test
{
    public class NavigationTestGod : TestGodBase
    {
        private ICameraAdjuster _cameraAdjuster;
        protected ICamera _camera;
        protected ICameraCalculatorSettings _cameraCalculatorSettings;

        public float normalSmoothTime = 0.15f;
        public float slowSmoothTime = 0.5f;

        protected override void Setup(Helper helper)
        {
            _camera = new CameraBC(Camera.main);
            _cameraCalculatorSettings
                = new CameraCalculatorSettings(
                    Substitute.For<ISettingsManager>(),
                    _camera.Aspect);
            ICameraCalculator cameraCalculator = new CameraCalculator(_camera, _cameraCalculatorSettings);

            IUserInputCameraTargetProvider scrollWheelCameraTargetProvider
                = new ScrollWheelCameraTargetProvider(
                    InputBC.Instance,
                    _updaterProvider.PerFrameUpdater,
                    new ZoomCalculator(
                        _camera,
                        TimeBC.Instance,
                        _cameraCalculatorSettings.ValidOrthographicSizes,
                        BCUtils.ZoomScale.SCROLL_WHEEL,
                        zoomSettingsMultiplier: 1),
                    new DirectionalZoom(
                        _camera,
                        cameraCalculator,
                        _cameraCalculatorSettings.ValidOrthographicSizes));

            IStaticCameraTargetProvider defaultCameraTargetProvider = new StaticCameraTargetProvider(priority: 1);
            CameraTarget target 
                = new CameraTarget(
                    position: new Vector3(-35, 0, _camera.Position.z), 
                    orthographicSize: _cameraCalculatorSettings.ValidOrthographicSizes.Min);
            defaultCameraTargetProvider.SetTarget(target);

            ICameraTargetProvider cameraTargetProvider
                = new CompositeCameraTargetProvider(
                    defaultCameraTargetProvider,
                    new List<IUserInputCameraTargetProvider>() { scrollWheelCameraTargetProvider });

            // Instant, jerky adjuster
            _cameraAdjuster = new InstantCameraAdjuster(cameraTargetProvider, _camera);

            // Smooth adjuster
            ITime time = TimeBC.Instance;
            ICameraSmoothTimeProvider smoothTimeProvider = new CameraTransitionSpeedManager(normalSmoothTime, slowSmoothTime);
            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(_camera, time, smoothTimeProvider),
                    new SmoothPositionAdjuster(_camera, time, smoothTimeProvider));
        }

        protected virtual void Update()
        {
            _cameraAdjuster?.AdjustCamera();
            //Debug.Log("Camera position: " + _camera.Position + "  Orthographic size: " + _camera.OrthographicSize);
        }
    }
}