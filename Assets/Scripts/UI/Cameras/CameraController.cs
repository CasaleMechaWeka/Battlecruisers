using System;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
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
		private Vector3 _cameraPositionChangeVelocity = Vector3.zero;
		private float _cameraOrthographicSizeChangeVelocity;
        private ISettingsManager _settingsManager;
        private IFilter _shouldNavigationBeEnabledFilter;

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

            _cameraOrthographicSizeChangeVelocity = 0;

			_cameraState = CameraState.Overview;

            _cameraCalculator = new CameraCalculator(_camera);

			// Camera starts in overiview (ish, y-position is only roughly right :P)
			Vector3 overviewTargetPosition = transform.position;
			overviewTargetPosition.y = _cameraCalculator.FindCameraYPosition(_camera.orthographicSize);
			IList<CameraState> overviewInstants = new List<CameraState>();
			_overviewTarget = new CameraTarget(overviewTargetPosition, _camera.orthographicSize, CameraState.Overview, overviewInstants);

			// Player cruiser view
			float playerCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(playerCruiser);
            Vector3 playerCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(playerCruiser, playerCruiserOrthographicSize, transform.position.z);
			IList<CameraState> leftSideInstants = new List<CameraState> 
			{
				CameraState.RightMid,
				CameraState.AiCruiser
			};
			_playerCruiserTarget = new CameraTarget(playerCruiserTargetPosition, playerCruiserOrthographicSize, CameraState.PlayerCruiser, leftSideInstants);

			// Ai cruiser overview
			float aiCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(aiCruiser);
            Vector3 aiCruiserTargetPosition = _cameraCalculator.FindCruiserCameraPosition(aiCruiser, aiCruiserOrthographicSize, transform.position.z);
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
            else if (_shouldNavigationBeEnabledFilter.IsMatch)
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

        // IPAD:  Adapt input for IPad :P
        private void HandleUserInput()
        {
            bool inZoom = HandleZoom();
            bool inDrag = HandleDrag();
            // FELIX
			bool inScroll = HandleScroll();
			//bool inScroll = false;

			if ((inZoom || inDrag || inScroll)
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

        // FELIX  Move to own class, test
        /// <returns><c>true</c>, if in zoom, <c>false</c> otherwise.</returns>
        private bool HandleZoom()
        {
            bool inZoom = false;
            float yScrollDelta = Input.mouseScrollDelta.y;

            if (yScrollDelta != 0)
            {
                inZoom = true;

				// FELIX  Take timeDelta into consideration :/
                _camera.orthographicSize -= _settingsManager.ZoomSpeed * yScrollDelta;

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

        // FELIX  Comment out.  Keep logic for when I implement IPad scrolling?
		// IPAD  Implement scrolling via dragging :)
		// IPAD  Move to own class, test
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
                transform.position = EnforceCameraBounds(desiredCameraPosition);
            }
            else if (Input.GetMouseButtonUp(DRAGGING_MOUSE_BUTTON_INDEX))
            {
                // Drag end
                _inDrag = false;
            }

            return _inDrag;
        }

		// FELIX  Inject into new class?  Or const?
		private float _scrollBoundaryInPixels = 0;
		//private float _scrollBoundaryInPixels = 50;

		// FELIX  Move to own class, test
        /// <returns><c>true</c>, if in scroll, <c>false</c> otherwise.</returns>
        private bool HandleScroll()
		{
			float scrollSpeed = _cameraCalculator.FindScrollSpeed(_camera.orthographicSize);

            Vector3 desiredPosition
                = new Vector3(
    				FindDesiredX(transform.position, Input.mousePosition, scrollSpeed),
    				FindDesiredY(transform.position, Input.mousePosition, scrollSpeed),
                    transform.position.z);

            Vector3 clampedDesiredPosition = EnforceCameraBounds(desiredPosition);

			Debug.Log("Input.mousePosition: " + Input.mousePosition + "  camera position: " + transform.position + "  desiredPosition: " + desiredPosition + " clampedPosition " + clampedDesiredPosition);

			if (clampedDesiredPosition != transform.position)
			{
				transform.position = clampedDesiredPosition;
				return true;
			}

			return false;
        }

        private float FindDesiredX(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeed)
        {
			Debug.Log("Calculated scroll speed: " + scrollSpeed);

			// FELIX
			scrollSpeed *= Time.deltaTime;
			//scrollSpeed = 5 * Time.deltaTime;
			Debug.Log("Hardcoded scroll speed: " + scrollSpeed);

			if (mousePosition.x > Screen.width - _scrollBoundaryInPixels)
			{
				return cameraPosition.x + scrollSpeed;
			}
			else if (mousePosition.x < 0 + _scrollBoundaryInPixels)
			{
				return cameraPosition.x - scrollSpeed;
			}
			return cameraPosition.x;
		}

		private float FindDesiredY(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeed)
		{
			// FELIX
			scrollSpeed *= Time.deltaTime;
            //scrollSpeed = 5 * Time.deltaTime;

			if (mousePosition.y > Screen.height - _scrollBoundaryInPixels)
            {
				return cameraPosition.y + scrollSpeed;
            }
			else if (mousePosition.y < 0 + _scrollBoundaryInPixels)
            {
				return cameraPosition.y - scrollSpeed;
            }
			return transform.position.y;
		}

        private Vector3 EnforceCameraBounds(Vector3 desiredCameraPosition)
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
