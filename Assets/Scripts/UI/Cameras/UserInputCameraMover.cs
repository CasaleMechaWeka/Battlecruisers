using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
	/// <summary>
	/// Handles camera movement in response to user input:
	/// + Scrolling => Via mouse at screen edge
	/// + Zooming   => Via mouse scroll wheel
	/// </summary>
    /// FELIX  Update tests :)
	public class UserInputCameraMover : CameraMover, IUserInputCameraMover
	{
		private readonly ICamera _camera;
		private readonly IInput _input;
		private readonly IScrollHandler _scrollHandler;
        private readonly IMouseZoomHandler _zoomHandler;
		private readonly INavigationSettings _navigationSettings;

		public event EventHandler Zoomed;
		public event EventHandler Scrolled;

		public UserInputCameraMover(
			ICamera camera, 
			IInput input, 
			IScrollHandler scrollHandler, 
			IMouseZoomHandler zoomHandler, 
			INavigationSettings navigationSettings)
		{
			Helper.AssertIsNotNull(camera, input, scrollHandler, zoomHandler, navigationSettings);

			_camera = camera;
			_input = input;
			_scrollHandler = scrollHandler;
			_zoomHandler = zoomHandler;
			_navigationSettings = navigationSettings;
		}

		public override void MoveCamera(float deltaTime)
		{
			if (!_navigationSettings.IsUserInputEnabled)
			{
				return;
			}

            bool inZoom = HandleZoom();

            bool inScroll = false;
            if (!inZoom)
            {
			    inScroll = HandleScroll();
            }

			if ((inScroll || inZoom)
			    && State != CameraState.UserInputControlled)
            {
				State = CameraState.UserInputControlled;
            }
		}

        /// <returns><c>true</c>, if in zoom, <c>false</c> otherwise.</returns>
        private bool HandleZoom()
        {
            Vector3 zoomTargetWorldPosition = _camera.ScreenToWorldPoint(_input.MousePosition);
            Debug.Log("zoomTargetWorldPosition: " + zoomTargetWorldPosition);
            MouseZoomResult zoomResult = _zoomHandler.HandleZoom(zoomTargetWorldPosition, _input.MouseScrollDelta.y);

            // Scroll for directional zoom :D
            if (zoomResult.CameraPosition != _camera.Position)
            {
                Debug.Log("UserInputCameraMover zoomResult.CameraPosition: " + zoomResult.CameraPosition);

                _camera.Position = zoomResult.CameraPosition;
            }

            // Update zoom level
            if (!Mathf.Approximately(zoomResult.CameraOrthographicSize, _camera.OrthographicSize))
            {
                _camera.OrthographicSize = zoomResult.CameraOrthographicSize;

                if (Zoomed != null)
                {
                    Zoomed.Invoke(this, EventArgs.Empty);
                }

                return true;
            }

            return false;
        }

        /// <returns><c>true</c>, if in scroll, <c>false</c> otherwise.</returns>
        private bool HandleScroll()
        {
            Vector3 desiredPosition = _scrollHandler.FindCameraPosition(_camera.OrthographicSize, _camera.Position, _input.MousePosition);

            if (desiredPosition != _camera.Position)
            {
                _camera.Position = desiredPosition;

                if (Scrolled != null)
                {
                    Scrolled.Invoke(this, EventArgs.Empty);
                }

                return true;
            }

            return false;
        }
    }
}
