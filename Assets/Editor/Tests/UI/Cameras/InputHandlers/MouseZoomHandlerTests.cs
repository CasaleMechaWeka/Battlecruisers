using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils.Clamping;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.InputHandlers
{
    public class MouseZoomHandlerTests
    {
        private IMouseZoomHandler _zoomHandler;
        private ICamera _camera;
        private ISettingsManager _settingsManager;
        private IDeltaTimeProvider _deltaTimeProvider;
        private ICameraCalculator _calculator;
        private IPositionClamper _cameraPositionClamper;
        private Vector3 _zoomWorldTargetPosition;

        [SetUp]
        public void SetuUp()
        {
            _camera = Substitute.For<ICamera>();
            _camera.OrthographicSize.Returns(10);
            _camera.Aspect.Returns(1.3333f);
            _camera.Transform.Position.Returns(new Vector3(-1, -1, -10));

            _settingsManager = Substitute.For<ISettingsManager>();
            _settingsManager.ZoomSpeed.Returns(0.5f);

            _deltaTimeProvider = Substitute.For<IDeltaTimeProvider>();
            // * ZOOM_SPEED_MULTIPLIER = 1 :)
            _deltaTimeProvider.UnscaledDeltaTime.Returns(0.03333333f);

            _calculator = Substitute.For<ICameraCalculator>();

            _cameraPositionClamper = Substitute.For<IPositionClamper>();

            ICameraCalculatorSettings cameraCalculatorSettings = new CameraCalculatorSettings(_settingsManager, _camera.Aspect);

            _zoomHandler
                = new MouseZoomHandler(
                    _camera,
                    _settingsManager,
                    _deltaTimeProvider,
                    _calculator,
                    _cameraPositionClamper,
                    cameraCalculatorSettings.OrthographicSize);

            _zoomWorldTargetPosition = new Vector3(-5, 3, 0);
        }

        [Test]
        public void HandleZoom_NoZoom()
        {
            float yScroll = 0;
            MouseZoomResult expectedResult = new MouseZoomResult(_camera.OrthographicSize, _camera.Transform.Position);
            MouseZoomResult actualResult = _zoomHandler.HandleZoom(_zoomWorldTargetPosition, yScroll);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void HandleZoom_ZoomOut()
        {
            float yScroll = -5;
            float expectedSize = _camera.OrthographicSize - _settingsManager.ZoomSpeed * yScroll;
            MouseZoomResult expectedResult = new MouseZoomResult(expectedSize, _camera.Transform.Position);
            MouseZoomResult actualResult = _zoomHandler.HandleZoom(_zoomWorldTargetPosition, yScroll);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void HandleZoom_ZoomIn()
        {
            float yScroll = 5;
            float expectedSize = _camera.OrthographicSize - _settingsManager.ZoomSpeed * yScroll;

            Vector3 targetViewportPosition = new Vector3(0.1f, 0.9f, 0);
            _camera.WorldToViewportPoint(_zoomWorldTargetPosition).Returns(targetViewportPosition);

            Vector3 unclampedNewCameraPosition = new Vector3(7, 4, 3);
            _calculator
                .FindZoomingCameraPosition(
                    _zoomWorldTargetPosition,
                    targetViewportPosition,
                    expectedSize,
                    _camera.Aspect,
                    _camera.Transform.Position.z)
                .Returns(unclampedNewCameraPosition);

            Vector3 clampedNewCameraPosition = new Vector3(4, 4, 3);
            _cameraPositionClamper.Clamp(unclampedNewCameraPosition).Returns(clampedNewCameraPosition);

            MouseZoomResult expectedResult = new MouseZoomResult(expectedSize, clampedNewCameraPosition);
            MouseZoomResult actualResult = _zoomHandler.HandleZoom(_zoomWorldTargetPosition, yScroll);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}