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
		private ICameraMover _mover;

		private ICamera _camera;
        private IInput _input;
        private IScrollHandler _scrollHandler;
        private IMouseZoomHandler _zoomHandler;
		private CameraStateChangedArgs _lastArgs;
		private float _deltaTime;
		private float _sameOrthographicSize, _differentOrthographicSize;
		private Vector3 _samePosition, _differentPosition;

        [SetUp]
        public void SetuUp()
        {
			_camera = Substitute.For<ICamera>();
			_input = Substitute.For<IInput>();
			_scrollHandler = Substitute.For<IScrollHandler>();
			_zoomHandler = Substitute.For<IMouseZoomHandler>();

			_mover = new UserInputCameraMover(_camera, _input, _scrollHandler, _zoomHandler);

			_lastArgs = null;
			_mover.StateChanged += (sender, e) => _lastArgs = e;

            _samePosition = new Vector3(1, 2, 3);
            _differentPosition = new Vector3(3, 2, 1);
            _input.MousePosition.Returns(_samePosition);
			_input.MouseScrollDelta.Returns(new Vector2(5, 4));

			_sameOrthographicSize = 72.1f;
			_differentOrthographicSize = 12.7f;
			_camera.OrthographicSize = _sameOrthographicSize;
			_camera.Position = _samePosition;

			_deltaTime = 0.123f;
        }

        [Test]
		public void State()
        {
			Assert.AreEqual(CameraState.UserInputControlled, _mover.State);
        }

		#region MoveCamera
		[Test]
		public void MoveCamera_InZoom_InScroll_CurrentState_UserInput()
        {
			Scroll(shouldScroll: true);
            Zoom(shouldZoom: true);

			_mover.MoveCamera(_deltaTime, CameraState.UserInputControlled);

			ReceivedScroll();
            ReceivedZoom();

			Assert.AreEqual(_differentOrthographicSize, _camera.OrthographicSize);
			Assert.AreEqual(_differentPosition, _camera.Position);
			Assert.IsNull(_lastArgs);
        }

		[Test]
        public void MoveCamera_InZoom_InScroll_CurrentState_NotUserInput()
        {
			Scroll(shouldScroll: true);
            Zoom(shouldZoom: true);

			_mover.MoveCamera(_deltaTime, CameraState.Overview);

            ReceivedScroll();
            ReceivedZoom();

            Assert.AreEqual(_differentOrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(_differentPosition, _camera.Position);
			Assert.IsNotNull(_lastArgs);
			Assert.AreEqual(CameraState.UserInputControlled, _lastArgs.NewState);
        }

		[Test]
        public void MoveCamera_NotInZoom_NotInScroll()
        {
			Scroll(shouldScroll: false);
            Zoom(shouldZoom: false);

            _mover.MoveCamera(_deltaTime, CameraState.UserInputControlled);

            ReceivedScroll();
            ReceivedZoom();

			Assert.AreEqual(_sameOrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(_samePosition, _camera.Position);
            Assert.IsNull(_lastArgs);
        }

		[Test]
        public void MoveCamera_InZoom_CurrentState_NotUserInput()
        {
			Scroll(shouldScroll: false);
            Zoom(shouldZoom: true);

            _mover.MoveCamera(_deltaTime, CameraState.UserInputControlled);

            ReceivedScroll();
            ReceivedZoom();

            Assert.AreEqual(_differentOrthographicSize, _camera.OrthographicSize);
			Assert.AreEqual(_samePosition, _camera.Position);
            Assert.IsNull(_lastArgs);
        }

        [Test]
        public void MoveCamera_InScroll_CurrentState_NotUserInput()
        {
			Scroll(shouldScroll: true);
            Zoom(shouldZoom: false);

            _mover.MoveCamera(_deltaTime, CameraState.UserInputControlled);

            ReceivedScroll();
            ReceivedZoom();

            Assert.AreEqual(_sameOrthographicSize, _camera.OrthographicSize);
            Assert.AreEqual(_differentPosition, _camera.Position);
            Assert.IsNull(_lastArgs);
        }
		#endregion MoveCamera

		private void Zoom(bool shouldZoom)
        {
            float desiredOrthographicSize = shouldZoom ? _differentOrthographicSize : _sameOrthographicSize;
            _zoomHandler.FindCameraOrthographicSize(_camera.OrthographicSize, _input.MouseScrollDelta.y, _deltaTime).Returns(desiredOrthographicSize);
        }

		private void ReceivedZoom()
        {
			_zoomHandler.Received().FindCameraOrthographicSize(_sameOrthographicSize, _input.MouseScrollDelta.y, _deltaTime);
        }

        private void Scroll(bool shouldScroll)
        {
            Vector3 desiredPosition = shouldScroll ? _differentPosition : _samePosition;
            _scrollHandler.FindCameraPosition(_camera.OrthographicSize, _camera.Position, _input.MousePosition, _deltaTime).Returns(desiredPosition);
        }

        private void ReceivedScroll()
		{
			_scrollHandler.Received().FindCameraPosition(_sameOrthographicSize, _samePosition, _input.MousePosition, _deltaTime);
		}
    }
}
