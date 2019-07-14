using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras.Targets.Providers
{
    public class ScrollWheelCameraTargetProviderTests
    {
        private IUserInputCameraTargetProvider _cameraTargetProvider;

        private ICamera _camera;
        private ICameraCalculator _cameraCalculator;
        private IInput _input;
        private IRange<float> _validOrthographicSizes;
        private IUpdater _updater;
        private IZoomCalculator _zoomCalculator;

        private int _targetChangedCount, _userInputStartedCount, _userInputEndedCount;
        private float _zoomDelta;

        [SetUp]
        public void TestSetup()
        {
            _camera = Substitute.For<ICamera>();
            _cameraCalculator = Substitute.For<ICameraCalculator>();
            _input = Substitute.For<IInput>();
            _validOrthographicSizes = new Range<float>(5, 40);
            _updater = Substitute.For<IUpdater>();
            _zoomCalculator = Substitute.For<IZoomCalculator>();

            _camera.Transform.Position.Returns(new Vector3(1, 2, 3));
            _camera.OrthographicSize.Returns(17);
            _camera.Aspect.Returns(1.33f);

            _cameraTargetProvider
                = new ScrollWheelCameraTargetProvider(
                    _camera,
                    _cameraCalculator,
                    _input,
                    _validOrthographicSizes,
                    _updater,
                    _zoomCalculator);

            _targetChangedCount = 0;
            _cameraTargetProvider.TargetChanged += (sender, e) => _targetChangedCount++;

            _userInputStartedCount = 0;
            _cameraTargetProvider.UserInputStarted += (sender, e) => _userInputStartedCount++;

            _userInputEndedCount = 0;
            _cameraTargetProvider.UserInputEnded += (sender, e) => _userInputEndedCount++;

            _zoomDelta = 5;
            _zoomCalculator.FindZoomDelta(default).ReturnsForAnyArgs(_zoomDelta);

            _input.MousePosition.Returns(new Vector3(99, 98, 97));
        }

        [Test]
        public void InitialState()
        {
            ICameraTarget expectedTarget = new CameraTarget(_camera.Transform.Position, _camera.OrthographicSize);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_StarteUserInput()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);
        }

        [Test]
        public void Update_UserInput_DuringUserInput()
        {
            // First user input
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);

            // Second user input, event not raised again
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);
        }

        [Test]
        public void Update_NoUserInput()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 0));

            _updater.Updated += Raise.Event();

            Assert.AreEqual(0, _targetChangedCount);
            Assert.AreEqual(0, _userInputEndedCount);
        }

        [Test]
        public void Update_NoUserInput_DuringUserInput()
        {
            // Start user input
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputStartedCount);

            // End user input
            _input.MouseScrollDelta.Returns(new Vector2(0, 0));
            _updater.Updated += Raise.Event();
            Assert.AreEqual(1, _userInputEndedCount);
        }

        [Test]
        public void Update_ZoomOut_WithinClamps()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, -1));

            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Min);
            float targetOrthographicSize = _validOrthographicSizes.Min + _zoomDelta;

            // Find target camera x position
            IRange<float> validXPositions = new Range<float>(_camera.Transform.Position.x - 5, _camera.Transform.Position.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = _camera.Transform.Position.x;

            // Find target camera y position
            float targetYPosition = 17;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(17);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_ZoomOut_UpperClamps()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, -1));

            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Max);
            float targetOrthographicSize = _validOrthographicSizes.Max;

            // Find target camera x position
            IRange<float> validXPositions = new Range<float>(_camera.Transform.Position.x - 5, _camera.Transform.Position.x - 1);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Max;

            // Find target camera y position
            float targetYPosition = 17;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(17);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_ZoomOut_LowerClamps()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, -1));

            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Min - _zoomDelta - 1);
            float targetOrthographicSize = _validOrthographicSizes.Min;

            // Find target camera x position
            IRange<float> validXPositions = new Range<float>(_camera.Transform.Position.x + 1, _camera.Transform.Position.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Min;

            // Find target camera y position
            float targetYPosition = 17;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(17);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_ZoomIn_WithinClamps()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));

            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Max);
            float targetOrthographicSize = _validOrthographicSizes.Max - _zoomDelta;

            // Find target camera x position, zoom towards mouse
            Vector3 mouseWorldPosition = new Vector3(-1, -2, -3);
            _camera.ScreenToWorldPoint(_input.MousePosition).Returns(mouseWorldPosition);
            Vector3 mouseViewportPosition = new Vector3(44, 55, 66);
            _camera.WorldToViewportPoint(mouseWorldPosition).Returns(mouseViewportPosition);

            Vector3 mouseZoomPosition = new Vector3(5, 6, 7);
            _cameraCalculator
                .FindZoomingCameraPosition(
                    mouseWorldPosition,
                    mouseViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Transform.Position.z)
                .Returns(mouseZoomPosition);

            IRange<float> validXPositions = new Range<float>(mouseZoomPosition.x - 5, mouseZoomPosition.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = mouseZoomPosition.x;

            // Find target camera y position
            float targetYPosition = 71;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(targetYPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_ZoomIn_UpperClamps()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));

            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Max + _zoomDelta + 1);
            float targetOrthographicSize = _validOrthographicSizes.Max;

            // Find target camera x position, zoom towards mouse
            Vector3 mouseWorldPosition = new Vector3(-1, -2, -3);
            _camera.ScreenToWorldPoint(_input.MousePosition).Returns(mouseWorldPosition);
            Vector3 mouseViewportPosition = new Vector3(44, 55, 66);
            _camera.WorldToViewportPoint(mouseWorldPosition).Returns(mouseViewportPosition);

            Vector3 mouseZoomPosition = new Vector3(5, 6, 7);
            _cameraCalculator
                .FindZoomingCameraPosition(
                    mouseWorldPosition,
                    mouseViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Transform.Position.z)
                .Returns(mouseZoomPosition);

            IRange<float> validXPositions = new Range<float>(mouseZoomPosition.x - 5, mouseZoomPosition.x - 1);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Max;

            // Find target camera y position
            float targetYPosition = 71;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(targetYPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }

        [Test]
        public void Update_ZoomIn_LowerClamps()
        {
            _input.MouseScrollDelta.Returns(new Vector2(0, 1));

            // Find target camera orthographic size
            _camera.OrthographicSize.Returns(_validOrthographicSizes.Min);
            float targetOrthographicSize = _validOrthographicSizes.Min;

            // Find target camera x position, zoom towards mouse
            Vector3 mouseWorldPosition = new Vector3(-1, -2, -3);
            _camera.ScreenToWorldPoint(_input.MousePosition).Returns(mouseWorldPosition);
            Vector3 mouseViewportPosition = new Vector3(44, 55, 66);
            _camera.WorldToViewportPoint(mouseWorldPosition).Returns(mouseViewportPosition);

            Vector3 mouseZoomPosition = new Vector3(5, 6, 7);
            _cameraCalculator
                .FindZoomingCameraPosition(
                    mouseWorldPosition,
                    mouseViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Transform.Position.z)
                .Returns(mouseZoomPosition);

            IRange<float> validXPositions = new Range<float>(mouseZoomPosition.x + 1, mouseZoomPosition.x + 5);
            _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize).Returns(validXPositions);
            float targetXPosition = validXPositions.Min;

            // Find target camera y position
            float targetYPosition = 71;
            _cameraCalculator.FindCameraYPosition(targetOrthographicSize).Returns(targetYPosition);

            ICameraTarget expectedTarget
                = new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);

            _updater.Updated += Raise.Event();

            Assert.AreEqual(1, _targetChangedCount);
            Assert.AreEqual(expectedTarget, _cameraTargetProvider.Target);
        }
    }
}