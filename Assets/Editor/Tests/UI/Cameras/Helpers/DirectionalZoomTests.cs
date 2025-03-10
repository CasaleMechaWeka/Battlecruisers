using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Helpers
{
    public class DirectionalZoomTests
    {
        private IDirectionalZoom _directionalZoom;

        private ICamera _camera;
        private ICameraCalculator _cameraCalculator;
        private IRange<float> _validOrthographicSizes;

        private readonly float _orthographicSizeDelta = 5;
        private readonly Vector3 _contactPosition = new Vector3(97, 98, 99);

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _cameraCalculator = Substitute.For<ICameraCalculator>();
            _validOrthographicSizes = new Range<float>(5, 40);

            _camera.Position.Returns(new Vector3(1, 2, 3));
            _camera.OrthographicSize.Returns(17);
            _camera.Aspect.Returns(1.33f);

            _directionalZoom = new DirectionalZoom(_camera, _cameraCalculator, _validOrthographicSizes);
        }

        #region ZoomOut()
        [Test]
        public void Update_ZoomOut_WithinClamps()
        {
            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Min);
            float targetOrthographicSize = _validOrthographicSizes.Min + _orthographicSizeDelta;

            // Find target camera x position
            IRange<float> validXPositions = new Range<float>(_camera.Position.x - 5, _camera.Position.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = _camera.Position.x;

            // Find target camera y position
            float targetYPosition = 17;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(17);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);

            ICameraTarget actualTarget = _directionalZoom.ZoomOut(_orthographicSizeDelta);

            Assert.AreEqual(expectedTarget, actualTarget);
        }

        [Test]
        public void Update_ZoomOut_UpperClamps()
        {
            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Max);
            float targetOrthographicSize = _validOrthographicSizes.Max;

            // Find target camera x position
            IRange<float> validXPositions = new Range<float>(_camera.Position.x - 5, _camera.Position.x - 1);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Max;

            // Find target camera y position
            float targetYPosition = 17;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(17);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);

            ICameraTarget actualTarget = _directionalZoom.ZoomOut(_orthographicSizeDelta);

            Assert.AreEqual(expectedTarget, actualTarget);
        }

        [Test]
        public void Update_ZoomOut_LowerClamps()
        {
            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Min - _orthographicSizeDelta - 1);
            float targetOrthographicSize = _validOrthographicSizes.Min;

            // Find target camera x position
            IRange<float> validXPositions = new Range<float>(_camera.Position.x + 1, _camera.Position.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Min;

            // Find target camera y position
            float targetYPosition = 17;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(17);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);

            ICameraTarget actualTarget = _directionalZoom.ZoomOut(_orthographicSizeDelta);

            Assert.AreEqual(expectedTarget, actualTarget);
        }
        #endregion ZoomOut()

        #region ZoomIn()
        [Test]
        public void Update_ZoomIn_WithinClamps()
        {
            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Max);
            float targetOrthographicSize = _validOrthographicSizes.Max - _orthographicSizeDelta;

            // Find target camera x position, zoom towards contact
            Vector3 contactWorldPosition = new Vector3(-1, -2, -3);
            _camera.ScreenToWorldPoint(_contactPosition).Returns(contactWorldPosition);
            Vector3 contactViewportPosition = new Vector3(44, 55, 66);
            _camera.WorldToViewportPoint(contactWorldPosition).Returns(contactViewportPosition);

            Vector3 contactZoomPosition = new Vector3(5, 6, 7);
            _cameraCalculator
                .FindZoomingCameraPosition(
                    contactWorldPosition,
                    contactViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Position.z)
                .Returns(contactZoomPosition);

            IRange<float> validXPositions = new Range<float>(contactZoomPosition.x - 5, contactZoomPosition.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = contactZoomPosition.x;

            // Find target camera y position
            float targetYPosition = 71;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(targetYPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);

            ICameraTarget actualTarget = _directionalZoom.ZoomIn(_orthographicSizeDelta, _contactPosition);

            Assert.AreEqual(expectedTarget, actualTarget);
        }

        [Test]
        public void Update_ZoomIn_UpperClamps()
        {
            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Max + _orthographicSizeDelta + 1);
            float targetOrthographicSize = _validOrthographicSizes.Max;

            // Find target camera x position, zoom towards contact
            Vector3 contactWorldPosition = new Vector3(-1, -2, -3);
            _camera.ScreenToWorldPoint(_contactPosition).Returns(contactWorldPosition);
            Vector3 contactViewportPosition = new Vector3(44, 55, 66);
            _camera.WorldToViewportPoint(contactWorldPosition).Returns(contactViewportPosition);

            Vector3 contactZoomPosition = new Vector3(5, 6, 7);
            _cameraCalculator
                .FindZoomingCameraPosition(
                    contactWorldPosition,
                    contactViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Position.z)
                .Returns(contactZoomPosition);

            IRange<float> validXPositions = new Range<float>(contactZoomPosition.x - 5, contactZoomPosition.x - 1);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Max;

            // Find target camera y position
            float targetYPosition = 71;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(targetYPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);

            ICameraTarget actualTarget = _directionalZoom.ZoomIn(_orthographicSizeDelta, _contactPosition);

            Assert.AreEqual(expectedTarget, actualTarget);
        }

        [Test]
        public void Update_ZoomIn_LowerClamps()
        {
            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Min);
            float targetOrthographicSize = _validOrthographicSizes.Min;

            // Find target camera x position, zoom towards contact
            Vector3 contactWorldPosition = new Vector3(-1, -2, -3);
            _camera.ScreenToWorldPoint(_contactPosition).Returns(contactWorldPosition);
            Vector3 contactViewportPosition = new Vector3(44, 55, 66);
            _camera.WorldToViewportPoint(contactWorldPosition).Returns(contactViewportPosition);

            Vector3 contactZoomPosition = new Vector3(5, 6, 7);
            _cameraCalculator
                .FindZoomingCameraPosition(
                    contactWorldPosition,
                    contactViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Position.z)
                .Returns(contactZoomPosition);

            IRange<float> validXPositions = new Range<float>(contactZoomPosition.x + 1, contactZoomPosition.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Min;

            // Find target camera y position
            float targetYPosition = 71;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(targetYPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);

            ICameraTarget actualTarget = _directionalZoom.ZoomIn(_orthographicSizeDelta, _contactPosition);

            Assert.AreEqual(expectedTarget, actualTarget);
        }
        #endregion ZoomIn()
    }
}