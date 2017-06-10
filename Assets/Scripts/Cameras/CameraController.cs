using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cameras
{
	public enum CameraState
	{
		PlayerCruiser, AiCruiser, Overview, InTransition
	}

	public class CameraTransitionArgs : EventArgs
	{
		public CameraState Origin { get; private set; }
		public CameraState Destination  { get; private set; }

		public CameraTransitionArgs(CameraState origin, CameraState destination)
		{
			Origin = origin;
			Destination = destination;
		}
	}

	public interface ICameraController 
	{
		CameraState State { get; }

		void FocusOnPlayerCruiser();
		void FocusOnAiCruiser();
		void ShowFullMapView();
	}

	public class CameraController : MonoBehaviour, ICameraController 
	{
		private Camera _camera;
		private ICameraTarget _currentTarget, _playerCruiserTarget, _aiCruiserTarget, _overviewTarget, _leftTarget, _rightTarget;
		private Vector3 _cameraPositionChangeVelocity = Vector3.zero;
		private float _cameraOrthographicSizeChangeVelocity = 0;

		public float smoothTime;

		public event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		public event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;

		private CameraState _cameraState;
		public CameraState State { get { return _cameraState; } }

		public void Initialise(ICruiser playerCruiser, ICruiser aiCruiser)
		{
			_cameraState = CameraState.Overview;

			_camera = GetComponent<Camera>();
			Assert.IsNotNull(_camera);

			// Camera starts in overiview
			Vector3 overviewTargetPosition = transform.position;
			_overviewTarget = new CameraTarget(overviewTargetPosition, _camera.orthographicSize, CameraState.Overview);

			ICameraCalculator cameraCalculator = new CameraCalculator(_camera);

			float playerCruiserOrthographicSize = cameraCalculator.FindCameraOrthographicSize(playerCruiser);
			Vector3 playerCruiserTargetPosition = new Vector3(playerCruiser.Position.x, cameraCalculator.FindCameraYPosition(playerCruiserOrthographicSize), transform.position.z);
			_playerCruiserTarget = new CameraTarget(playerCruiserTargetPosition, playerCruiserOrthographicSize, CameraState.PlayerCruiser);

			float aiCruiserOrthographicSize = cameraCalculator.FindCameraOrthographicSize(aiCruiser);
			Vector3 aiCruiserTargetPosition = new Vector3(aiCruiser.Position.x, cameraCalculator.FindCameraYPosition(aiCruiserOrthographicSize), transform.position.z);
			_aiCruiserTarget = new CameraTarget(aiCruiserTargetPosition, aiCruiserOrthographicSize, CameraState.AiCruiser);


			FocusOnPlayerCruiser();
		}

		void Update()
		{
			if (_cameraState != _currentTarget.State)
			{
				// Camera position
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

				// Camera zoom
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
		}

		public void FocusOnPlayerCruiser()
		{
			bool isInstant = _cameraState != CameraState.Overview;
			_currentTarget = _playerCruiserTarget;
			MoveCamera(isInstant);
		}

		public void FocusOnAiCruiser()
		{
			bool isInstant = _cameraState != CameraState.Overview;
			_currentTarget = _aiCruiserTarget;
			MoveCamera(isInstant);
		}

		public void ShowFullMapView()
		{
			_currentTarget = _overviewTarget;
			MoveCamera(isInstant: false);
		}

		/// <returns><c>true</c>, if camera was or will be moved, <c>false</c> otherwise.</returns>
		private bool MoveCamera(bool isInstant)
		{
			bool willMoveCamera = 
				_cameraState != CameraState.InTransition
			    && _cameraState != _currentTarget.State;

			Logging.Log(Tags.CAMERA_CONTROLLER, "MoveCamera _currentTarget.State: " + _currentTarget.State + "  willMoveCamera: " + willMoveCamera + "  _cameraState: " + _cameraState);

			if (willMoveCamera)
			{
				if (CameraTransitionStarted != null)
				{
					CameraTransitionStarted.Invoke(this, new CameraTransitionArgs(_cameraState, _currentTarget.State));
				}

				if (isInstant)
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
