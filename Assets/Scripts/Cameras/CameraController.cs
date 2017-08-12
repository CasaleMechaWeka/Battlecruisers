using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cameras
{
	public class CameraController : MonoBehaviour, ICameraController
	{
		private Camera _camera;
        private ICameraCalculator _cameraCalculator;
        private ICameraTarget _currentTarget, _playerCruiserTarget, _aiCruiserTarget, _overviewTarget, _midLeftTarget, _midRightTarget, _playerInputTarget;
		private Vector3 _cameraPositionChangeVelocity = Vector3.zero;
		private float _cameraOrthographicSizeChangeVelocity = 0;

        // Dragging
		private bool _inDrag;
		private Vector3 _dragStartCameraPosition;
		private Vector3 _dragStartMousePosition;

		public float smoothTime;

		public event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		public event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
		private const float MID_VIEWS_ORTHOGRAPHIC_SIZE = 18;
		private const float MID_VIEWS_POSITION_X = 20;
		private const float ZOOM_SPEED = 0.5f;

        // Dragging
        private const int DRAGGING_MOUSE_BUTTON_INDEX = 1;  // Primary mouse button
		private const float CAMERA_POSITION_MAX_X = 35;
		private const float CAMERA_POSITION_MIN_X = -35;
		private const float CAMERA_POSITION_MAX_Y = 30;
		private const float CAMERA_POSITION_MIN_Y = 0;

		private CameraState _cameraState;
		public CameraState State { get { return _cameraState; } }

		public void Initialise(ICruiser playerCruiser, ICruiser aiCruiser)
		{
			_camera = GetComponent<Camera>();
			Assert.IsNotNull(_camera);

			_cameraState = CameraState.Overview;

            _cameraCalculator = new CameraCalculator(_camera);

			// Camera starts in overiview (ish, y-position is only roughly right :P)
			Vector3 overviewTargetPosition = transform.position;
			overviewTargetPosition.y = _cameraCalculator.FindCameraYPosition(_camera.orthographicSize);
			IList<CameraState> overviewInstants = new List<CameraState>();
			_overviewTarget = new CameraTarget(overviewTargetPosition, _camera.orthographicSize, CameraState.Overview, overviewInstants);

			// Player cruiser view
			float playerCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(playerCruiser);
			Vector3 playerCruiserTargetPosition = new Vector3(playerCruiser.Position.x, _cameraCalculator.FindCameraYPosition(playerCruiserOrthographicSize), transform.position.z);
			IList<CameraState> leftSideInstants = new List<CameraState> 
			{
				CameraState.RightMid,
				CameraState.AiCruiser
			};
			_playerCruiserTarget = new CameraTarget(playerCruiserTargetPosition, playerCruiserOrthographicSize, CameraState.PlayerCruiser, leftSideInstants);

			// Ai cruiser overview
			float aiCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(aiCruiser);
			Vector3 aiCruiserTargetPosition = new Vector3(aiCruiser.Position.x, _cameraCalculator.FindCameraYPosition(aiCruiserOrthographicSize), transform.position.z);
			IList<CameraState> rightSideInstants = new List<CameraState> 
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
	                state: CameraState.PlayerInputControlled,
	                instantStates: new List<CameraState>());

			FocusOnPlayerCruiser();
		}

		void Update()
		{
			if (_cameraState != _currentTarget.State)
            {
                bool isInPosition = UpdateCameraPosition();
                bool isRightOrthographicSize = UpdateCameraZoom();

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
            else
            {
                HandleUserInput();
            }
		}

        private bool UpdateCameraPosition()
        {
            bool isInPosition = (transform.position - _currentTarget.Position).magnitude < POSITION_EQUALITY_MARGIN;
            if (!isInPosition)
            {
                transform.position = Vector3.SmoothDamp(transform.position, _currentTarget.Position, ref _cameraPositionChangeVelocity, smoothTime);
            }
            else if (transform.position != _currentTarget.Position)
            {
                transform.position = _currentTarget.Position;
                Logging.Log(Tags.CAMERA_CONTROLLER, "CameraController position done");
            }

            return isInPosition;
        }

		private bool UpdateCameraZoom()
		{
			bool isRightOrthographicSize = Math.Abs(_camera.orthographicSize - _currentTarget.OrthographicSize) < ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN;
			if (!isRightOrthographicSize)
			{
				_camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _currentTarget.OrthographicSize, ref _cameraOrthographicSizeChangeVelocity, smoothTime);
			}
			else if (_camera.orthographicSize != _currentTarget.OrthographicSize)
			{
				_camera.orthographicSize = _currentTarget.OrthographicSize;
				Logging.Log(Tags.CAMERA_CONTROLLER, "CameraController zoom done");
			}
			
			return isRightOrthographicSize;
		}

        // FELIX  Adapt for IPad :P
		private void HandleUserInput()
        {
            bool inZoom = HandleZoom();
            bool inDrag = HandleDrag();

            if ((inZoom || inDrag)
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
            bool inZoom = false;
            float yScrollDelta = Input.mouseScrollDelta.y;

            if (yScrollDelta != 0)
            {
                inZoom = true;

                _camera.orthographicSize -= ZOOM_SPEED * yScrollDelta;

                if (_camera.orthographicSize > CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE)
                {
                    _camera.orthographicSize = CameraCalculator.MAX_CAMERA_ORTHOGRAPHIC_SIZE;
                }
                else if (_camera.orthographicSize < CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE)
                {
                    _camera.orthographicSize = CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE;
                }
            }

            return inZoom;
        }

        /// <returns><c>true</c>, if in drag, <c>false</c> otherwise.</returns>
        private bool HandleDrag()
        {
            if (Input.GetMouseButtonDown(DRAGGING_MOUSE_BUTTON_INDEX))
            {
                // Drag start
                _inDrag = true;
                _dragStartCameraPosition = transform.position;
                _dragStartMousePosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(DRAGGING_MOUSE_BUTTON_INDEX))
            {
                // Mid drag
                Vector3 mousePositionDelta = _camera.ScreenToViewportPoint(Input.mousePosition) - _dragStartMousePosition;
                Vector3 desiredCameraPosition = _dragStartCameraPosition - mousePositionDelta * _cameraCalculator.FindScrollSpeed(_camera.orthographicSize);
                transform.position = EnforeCameraBounds(desiredCameraPosition);
            }
            else if (Input.GetMouseButtonUp(DRAGGING_MOUSE_BUTTON_INDEX))
            {
                // Drag end
                _inDrag = false;
            }

            return _inDrag;
        }

        private Vector3 EnforeCameraBounds(Vector3 desiredCameraPosition)
        {
            if (desiredCameraPosition.x < CAMERA_POSITION_MIN_X)
            {
                desiredCameraPosition.x = CAMERA_POSITION_MIN_X;
            }
            else if (desiredCameraPosition.x > CAMERA_POSITION_MAX_X)
            {
                desiredCameraPosition.x = CAMERA_POSITION_MAX_X;
            }

            if (desiredCameraPosition.y < CAMERA_POSITION_MIN_Y)
            {
                desiredCameraPosition.y = CAMERA_POSITION_MIN_Y;
            }
            else if (desiredCameraPosition.y > CAMERA_POSITION_MAX_Y)
            {
                desiredCameraPosition.y = CAMERA_POSITION_MAX_Y;
            }

            return desiredCameraPosition;
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
