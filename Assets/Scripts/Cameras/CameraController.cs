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
		private ICameraTarget _currentTarget, _playerCruiserTarget, _aiCruiserTarget, _overviewTarget, _midLeftTarget, _midRightTarget;
		private Vector3 _cameraPositionChangeVelocity = Vector3.zero;
		private float _cameraOrthographicSizeChangeVelocity = 0;

		public float smoothTime;

		public event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		public event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
		private const float MID_VIEWS_ORTHOGRAPHIC_SIZE = 18;
		private const float MID_VIEWS_POSITION_X = 20;
        private const float ZOOM_SPEED = 0.5f;

		private CameraState _cameraState;
		public CameraState State { get { return _cameraState; } }

		public void Initialise(ICruiser playerCruiser, ICruiser aiCruiser)
		{
			_camera = GetComponent<Camera>();
			Assert.IsNotNull(_camera);

			_cameraState = CameraState.Overview;

			ICameraCalculator cameraCalculator = new CameraCalculator(_camera);

			// Camera starts in overiview (ish, y-position is only roughly right :P)
			Vector3 overviewTargetPosition = transform.position;
			overviewTargetPosition.y = cameraCalculator.FindCameraYPosition(_camera.orthographicSize);
			IList<CameraState> overviewInstants = new List<CameraState>();
			_overviewTarget = new CameraTarget(overviewTargetPosition, _camera.orthographicSize, CameraState.Overview, overviewInstants);

			// Player cruiser view
			float playerCruiserOrthographicSize = cameraCalculator.FindCameraOrthographicSize(playerCruiser);
			Vector3 playerCruiserTargetPosition = new Vector3(playerCruiser.Position.x, cameraCalculator.FindCameraYPosition(playerCruiserOrthographicSize), transform.position.z);
			IList<CameraState> leftSideInstants = new List<CameraState> 
			{
				CameraState.RightMid,
				CameraState.AiCruiser
			};
			_playerCruiserTarget = new CameraTarget(playerCruiserTargetPosition, playerCruiserOrthographicSize, CameraState.PlayerCruiser, leftSideInstants);

			// Ai cruiser overview
			float aiCruiserOrthographicSize = cameraCalculator.FindCameraOrthographicSize(aiCruiser);
			Vector3 aiCruiserTargetPosition = new Vector3(aiCruiser.Position.x, cameraCalculator.FindCameraYPosition(aiCruiserOrthographicSize), transform.position.z);
			IList<CameraState> rightSideInstants = new List<CameraState> 
			{
				CameraState.LeftMid,
				CameraState.PlayerCruiser
			};
			_aiCruiserTarget = new CameraTarget(aiCruiserTargetPosition, aiCruiserOrthographicSize, CameraState.AiCruiser, rightSideInstants);

			float midViewsPositionY = cameraCalculator.FindCameraYPosition(MID_VIEWS_ORTHOGRAPHIC_SIZE);

			// Left mid view
			Vector3 leftMidViewPosition = new Vector3(-MID_VIEWS_POSITION_X, midViewsPositionY, transform.position.z);
			_midLeftTarget = new CameraTarget(leftMidViewPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.LeftMid, leftSideInstants);

			// Right mid view
			Vector3 rightMidPosition = new Vector3(MID_VIEWS_POSITION_X, midViewsPositionY, transform.position.z);
			_midRightTarget = new CameraTarget(rightMidPosition, MID_VIEWS_ORTHOGRAPHIC_SIZE, CameraState.RightMid, rightSideInstants);

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
            //Input.GetKey(KeyCode.LeftArrow);

            // FELIX  TEMP
            if (Input.mouseScrollDelta != default(Vector2))
            {
                Debug.Log("Input.mouseScrollDelta: " + Input.mouseScrollDelta);
			}

            float yScrollDelta = Input.mouseScrollDelta.y; 

            if (yScrollDelta != 0)
            {
                _camera.orthographicSize -= ZOOM_SPEED * yScrollDelta;
            }
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
