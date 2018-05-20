using System;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
	public class UserInputCameraMover : CameraMover
	{
		private readonly ICamera _camera;
		private readonly IInput _input;
		private readonly IScrollHandler _scrollHandler;
        private readonly IMouseZoomHandler _zoomHandler;

		public UserInputCameraMover(ICamera camera, IInput input, IScrollHandler scrollHandler, IMouseZoomHandler zoomHandler)
		{
			Helper.AssertIsNotNull(camera, input, scrollHandler, zoomHandler);

			_camera = camera;
			_input = input;
			_scrollHandler = scrollHandler;
			_zoomHandler = zoomHandler;
		}

		public override void MoveCamera(float deltaTime)
		{
			// Want to handle scrolling first, because zoom can change the camera
            // orthographic size, which affects scrolling.
			bool inScroll = HandleScroll(deltaTime);
            bool inZoom = HandleZoom(deltaTime);

			if ((inScroll || inZoom)
			    && State != CameraState.UserInputControlled)
            {
				State = CameraState.UserInputControlled;
            }
		}
  
        /// <returns><c>true</c>, if in scroll, <c>false</c> otherwise.</returns>
		private bool HandleScroll(float deltaTime)
        {
			Vector3 desiredPosition = _scrollHandler.FindCameraPosition(_camera.OrthographicSize, _camera.Position, _input.MousePosition, deltaTime);

			if (desiredPosition != _camera.Position)
            {
                _camera.Position = desiredPosition;
                return true;
            }

            return false;
        }

        /// <returns><c>true</c>, if in zoom, <c>false</c> otherwise.</returns>
        private bool HandleZoom(float deltaTime)
        {
            float desiredOrthographicSize = _zoomHandler.FindCameraOrthographicSize(_camera.OrthographicSize, _input.MouseScrollDelta.y, deltaTime);

            if (!Mathf.Approximately(desiredOrthographicSize, _camera.OrthographicSize))
            {
                _camera.OrthographicSize = desiredOrthographicSize;
                return true;
            }

            return false;
        }
	}
}
