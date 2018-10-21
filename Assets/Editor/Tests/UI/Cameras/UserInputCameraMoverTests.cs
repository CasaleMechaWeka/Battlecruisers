using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras
{
    public class UserInputCameraMoverTests
    {
        private IUserInputCameraMover _mover;

        private ICamera _camera;
        private IInput _input;
        private IScrollHandler _scrollHandler;
        private IMouseZoomHandler _zoomHandler;
        private INavigationSettings _navigationSettings;
        private CameraStateChangedArgs _lastArgs;
        private float _deltaTime;
        private float _sameOrthographicSize, _differentOrthographicSize;
        private Vector3 _samePosition, _differentPosition;
        private int _zoomCounter, _scrollCounter;
        private Vector3 _zoomTargetWorldPosition;

        [SetUp]
        public void SetuUp()
        {
            _camera = Substitute.For<ICamera>();
            _input = Substitute.For<IInput>();
            _scrollHandler = Substitute.For<IScrollHandler>();
            _zoomHandler = Substitute.For<IMouseZoomHandler>();

            _navigationSettings = Substitute.For<INavigationSettings>();
            _navigationSettings.IsUserInputEnabled.Returns(true);

            _mover = new UserInputCameraMover(_camera, _input, _scrollHandler, _zoomHandler, _navigationSettings);

            _lastArgs = null;
            _mover.StateChanged += (sender, e) => _lastArgs = e;

            _samePosition = new Vector3(1, 2, 3);
            _differentPosition = new Vector3(3, 2, 1);
            _input.MousePosition.Returns(_samePosition);
            _input.MouseScrollDelta.Returns(new Vector2(5, 4));

            _sameOrthographicSize = 72.1f;
            _differentOrthographicSize = 12.7f;
            _camera.OrthographicSize = _sameOrthographicSize;
            _camera.Transform.Position = _samePosition;

            _deltaTime = 0.123f;

            _zoomCounter = 0;
            _mover.Zoomed += (sender, e) => _zoomCounter++;

            _scrollCounter = 0;
            _mover.Scrolled += (sender, e) => _scrollCounter++;

            _zoomTargetWorldPosition = new Vector3(-5, 3, 0);
            _camera.ScreenToWorldPoint(_input.MousePosition).Returns(_zoomTargetWorldPosition);
        }

        [Test]
        public void State()
        {
            Assert.AreEqual(CameraState.UserInputControlled, _mover.State);
        }

        #region MoveCamera
        [Test]
        public void MoveCamere_WhileDisabled_DoesNothing()
        {
            _navigationSettings.IsUserInputEnabled.Returns(false);

            _mover.MoveCamera(_deltaTime);

            DidNotReceiveZoom();
            DidNotReceiveScroll();
        }

        [Test]
        public void MoveCamera_InZoom()
        {
            Zoom(shouldZoom: true);
            Scroll(shouldScroll: false);

            _mover.MoveCamera(_deltaTime);

            ReceivedZoom();
            DidNotReceiveScroll();

            Assert.AreEqual(_differentOrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(_differentPosition, _camera.Transform.Position);
            Assert.IsNull(_lastArgs);
            Assert.AreEqual(CameraState.UserInputControlled, _mover.State);
        }

        [Test]
        public void MoveCamera_InScroll()
        {
            Zoom(shouldZoom: false);
            Scroll(shouldScroll: true);

            _mover.MoveCamera(_deltaTime);

            ReceivedScroll();

            Assert.AreEqual(_sameOrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(_differentPosition, _camera.Transform.Position);
            Assert.IsNull(_lastArgs);
        }

        [Test]
        public void MoveCamera_WhileInZoom_DoesNotScroll()
        {
            Zoom(shouldZoom: true);
            Scroll(shouldScroll: true);

            _mover.MoveCamera(_deltaTime);

            ReceivedZoom();
            DidNotReceiveScroll();

            Assert.AreEqual(_differentOrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(_differentPosition, _camera.Transform.Position);
            Assert.IsNull(_lastArgs);
        }
        #endregion MoveCamera

        [Test]
        public void ScrollEventFired()
        {
            Scroll(shouldScroll: true);
            Zoom(shouldZoom: false);

            _mover.MoveCamera(_deltaTime);

            Assert.AreEqual(1, _scrollCounter);
            Assert.AreEqual(0, _zoomCounter);
        }

        [Test]
        public void ZoomEventFired()
        {
            Zoom(shouldZoom: true);
            Scroll(shouldScroll: false);

            _mover.MoveCamera(_deltaTime);

            Assert.AreEqual(1, _zoomCounter);
            Assert.AreEqual(0, _scrollCounter);
        }

        [Test]
        public void EventsNotFired()
        {
            Scroll(shouldScroll: false);
            Zoom(shouldZoom: false);

            _mover.MoveCamera(_deltaTime);

            Assert.AreEqual(0, _scrollCounter);
            Assert.AreEqual(0, _zoomCounter);
        }

        private void Zoom(bool shouldZoom)
        {
            float desiredOrthographicSize = shouldZoom ? _differentOrthographicSize : _sameOrthographicSize;
            Vector3 desiredCameraPosition = shouldZoom ? _differentPosition : _samePosition;
            MouseZoomResult zoomResult = new MouseZoomResult(desiredOrthographicSize, desiredCameraPosition);

            _zoomHandler.HandleZoom(_zoomTargetWorldPosition, _input.MouseScrollDelta.y).Returns(zoomResult);
        }

        private void ReceivedZoom()
        {
            _zoomHandler.Received().HandleZoom(_zoomTargetWorldPosition, _input.MouseScrollDelta.y);
        }

        private void DidNotReceiveZoom()
        {
            _zoomHandler.DidNotReceiveWithAnyArgs().HandleZoom(default(Vector3), default(float));
        }

        private void Scroll(bool shouldScroll)
        {
            Vector3 desiredPosition = shouldScroll ? _differentPosition : _samePosition;
            _scrollHandler.FindCameraPosition(_camera.OrthographicSize, _camera.Transform.Position, _input.MousePosition).Returns(desiredPosition);
        }

        private void ReceivedScroll()
        {
            _scrollHandler.Received().FindCameraPosition(_sameOrthographicSize, _samePosition, _input.MousePosition);
        }

        private void DidNotReceiveScroll()
        {
            _scrollHandler.DidNotReceiveWithAnyArgs().FindCameraPosition(default(float), default(Vector3), default(Vector3));
        }
    }
}
