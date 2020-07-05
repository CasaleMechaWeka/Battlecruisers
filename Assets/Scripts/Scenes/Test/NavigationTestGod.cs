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

        public float smoothTime = 0.15f;

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
                    new InputBC(),
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
            _cameraAdjuster
                = new SmoothCameraAdjuster(
                    cameraTargetProvider,
                    new SmoothZoomAdjuster(_camera, time, smoothTime),
                    new SmoothPositionAdjuster(_camera, time, smoothTime));
        }

        protected virtual void Update()
        {
            _cameraAdjuster?.AdjustCamera();
            //Debug.Log("Camera position: " + _camera.Position + "  Orthographic size: " + _camera.OrthographicSize);
        }
    }
}