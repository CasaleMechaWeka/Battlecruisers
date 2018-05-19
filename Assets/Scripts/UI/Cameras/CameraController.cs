using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.InputHandlers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
	// FELIX  Make this class testable :)
	public class CameraController : MonoBehaviour, ICameraController
	{
		private Camera _camera;
        private ICameraCalculator _cameraCalculator;
        private ICameraTarget _currentTarget, _playerCruiserTarget, _aiCruiserTarget, _overviewTarget, _midLeftTarget, _midRightTarget, _playerInputTarget;
        private ISettingsManager _settingsManager;
        private IFilter _shouldNavigationBeEnabledFilter;

		// Adjusting camera
		private ISmoothPositionAdjuster _positionAdjuster;
		private ISmoothZoomAdjuster _zoomAdjuster;

		// User input
		private IScrollHandler _scrollHandler;
		private IMouseZoomHandler _mouseZoomHandler;

		public float smoothTime;

		public event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		public event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

		private const float MID_VIEWS_ORTHOGRAPHIC_SIZE = 18;
		private const float MID_VIEWS_POSITION_X = 20;

        // Dragging
        private const int DRAGGING_MOUSE_BUTTON_INDEX = 1;  // Primary mouse button
		private const float CAMERA_POSITION_MAX_X = 35;
		private const float CAMERA_POSITION_MIN_X = -35;
		private const float CAMERA_POSITION_MAX_Y = 30;
		private const float CAMERA_POSITION_MIN_Y = 0;

		// FELIX  Just make property with private setter :/
		private CameraState _cameraState;
		public CameraState State { get { return _cameraState; } }

		public void Initialise(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ISettingsManager settingsManager, 
            Material skyboxMaterial,
            IFilter shouldNavigationBeEnabledFilter)
		{
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, settingsManager, skyboxMaterial, shouldNavigationBeEnabledFilter);

            _settingsManager = settingsManager;
            _shouldNavigationBeEnabledFilter = shouldNavigationBeEnabledFilter;

            _camera = GetComponent<Camera>();
            Assert.IsNotNull(_camera);
			
            Skybox skybox = GetComponent<Skybox>();
			Assert.IsNotNull(skybox);
			skybox.material = skyboxMaterial;

			_cameraState = CameraState.Overview;

            _cameraCalculator = new CameraCalculator(_camera);

			// Camera starts in overiview (ish, y-position is only roughly right :P)
			Vector3 overviewTargetPosition = transform.position;
			overviewTargetPosition.y = _cameraCalculator.FindCameraYPosition(_camera.orthographicSize);
			_overviewTarget = new CameraTarget(overviewTargetPosition, _camera.orthographicSize, CameraState.Overview);

			// Player cruiser view
			float playerCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(playerCruiser);
            Vector3 playerCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(playerCruiser, playerCruiserOrthographicSize, transform.position.z);
			CameraState[] leftSideInstants = 
			{
				CameraState.RightMid,
				CameraState.AiCruiser
			};
			_playerCruiserTarget = new CameraTarget(playerCruiserTargetPosition, playerCruiserOrthographicSize, CameraState.PlayerCruiser, leftSideInstants);

			// Ai cruiser overview
			float aiCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(aiCruiser);
            Vector3 aiCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(aiCruiser, aiCruiserOrthographicSize, transform.position.z);
			CameraState[] rightSideInstants =
			{
				CameraState.LeftMid,
				CameraState.PlayerCruiser
			};
			_aiCruiserTarget = new CameraTarget(aiCruiserTargetPosition, aiCruiserOrthographicSize, CameraState.AiCruiser, rightSideInstants);

			float midViewsPositionY = _cameraCalculator.FindCameraYPosition(MID_VIEWS_ORTHOGRAPHIC_SIZE);

			// Left mid view
			Vector3 leftMidViewPosition = new Vector3(-MID_VIEWS_POSITION_X, midViewsPositionY, transform.position.z);
			_midLeftTarget = new CameraTarget(leftMidViewPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.LeftMid, leftSideInstants);

			// Right mid view
			Vector3 rightMidPosition = new Vector3(MID_VIEWS_POSITION_X, midViewsPositionY, transform.position.z);
			_midRightTarget = new CameraTarget(rightMidPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.RightMid, rightSideInstants);

            // Player input controlled
            _playerInputTarget
                = new CameraTarget(
                    position: default(Vector3),
	                orthographicSize: -1,
	                state: CameraState.PlayerInputControlled);

			FocusOnPlayerCruiser();

			// FELIX  Move to factory and inject factory :)
			IScreen screen = new ScreenBC();
			Rectangle cameraBounds = new Rectangle(CAMERA_POSITION_MIN_X, CAMERA_POSITION_MAX_X, CAMERA_POSITION_MIN_Y, CAMERA_POSITION_MAX_Y);
			IPositionClamper cameraPositionClamper = new PositionClamper(cameraBounds);
			_scrollHandler = new ScrollHandler(_cameraCalculator, screen, cameraPositionClamper);

			_mouseZoomHandler = new MouseZoomHandler(_settingsManager, CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE, CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE);

			_positionAdjuster = new SmoothPositionAdjuster(transform, smoothTime);
			_zoomAdjuster = new SmoothZoomAdjuster(_camera, smoothTime);
		}

		void Update()
		{
			if (_cameraState != _currentTarget.State)
            {
				bool isInPosition = _positionAdjuster.AdjustPosition(_currentTarget.Position);
				bool isRightOrthographicSize = _zoomAdjuster.AdjustZoom(_currentTarget.OrthographicSize);

                // Camera state
                if (isInPosition && isRightOrthographicSize)
                {
                    if (CameraTransitionCompleted != null)
                    {
                        CameraTransitionCompleted.Invoke(this, new CameraTransitionArgs(_cameraState, _currentTarget.State));
                    }

                    _cameraState = _currentTarget.State;
                }
            }
            else if (_shouldNavigationBeEnabledFilter.IsMatch)
            {
                HandleUserInput();
            }
		}

        // IPAD:  Adapt input for IPad :P
        private void HandleUserInput()
        {
            bool inZoom = HandleZoom();
			bool inScroll = HandleScroll();

			if ((inZoom || inScroll)
			    && _currentTarget != _playerInputTarget)
			{
				if (CameraTransitionStarted != null)
				{
					CameraTransitionStarted.Invoke(this, new CameraTransitionArgs(_cameraState, _playerInputTarget.State));
				}
				
				_cameraState = _playerInputTarget.State;
				_currentTarget = _playerInputTarget;
			}
        }

        /// <returns><c>true</c>, if in zoom, <c>false</c> otherwise.</returns>
        private bool HandleZoom()
        {
			float desiredOrthographicSize = _mouseZoomHandler.FindCameraOrthographicSize(_camera.orthographicSize, Input.mouseScrollDelta.y, Time.deltaTime);

			if (!Mathf.Approximately(desiredOrthographicSize, _camera.orthographicSize))
			{
				_camera.orthographicSize = desiredOrthographicSize;
				return true;
			}

			return false;
        }

        /// <returns><c>true</c>, if in scroll, <c>false</c> otherwise.</returns>
        private bool HandleScroll()
		{
			Vector3 desiredPosition = _scrollHandler.FindCameraPosition(_camera.orthographicSize, transform.position, Input.mousePosition, Time.deltaTime);

			if (desiredPosition != transform.position)
			{
				transform.position = desiredPosition;
				return true;
			}

			return false;
        }

        public void FocusOnPlayerCruiser()
		{
			MoveCamera(_playerCruiserTarget);
		}

		public void FocusOnAiCruiser()
		{
			MoveCamera(_aiCruiserTarget);
		}

		public void ShowFullMapView()
		{
			MoveCamera(_overviewTarget);
		}

		public void ShowMidLeft()
		{
			MoveCamera(_midLeftTarget);
		}

		public void ShowMidRight()
		{
			MoveCamera(_midRightTarget);
		}

		/// <returns><c>true</c>, if camera was or will be moved, <c>false</c> otherwise.</returns>
		private bool MoveCamera(ICameraTarget newTarget)
		{
			bool willMoveCamera = 
				_cameraState != CameraState.InTransition
				&& _cameraState != newTarget.State;

			Logging.Log(Tags.CAMERA_CONTROLLER, "MoveCamera newTarget.State: " + newTarget.State + "  willMoveCamera: " + willMoveCamera + "  _cameraState: " + _cameraState);

			if (willMoveCamera)
			{
				_currentTarget = newTarget;

				if (CameraTransitionStarted != null)
				{
					CameraTransitionStarted.Invoke(this, new CameraTransitionArgs(_cameraState, _currentTarget.State));
				}

				if (_currentTarget.IsInstantTransition(_cameraState))
				{
					transform.position = _currentTarget.Position;
					_camera.orthographicSize = _currentTarget.OrthographicSize;
				}
				else
				{
					_cameraState = CameraState.InTransition;
				}
			}

			return willMoveCamera;
		}
	}
}
