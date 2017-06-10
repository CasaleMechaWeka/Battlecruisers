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
		PlayerCruiser, AiCruiser, Center, InTransition
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
		private ICruiser _playerCruiser;
		private ICruiser _aiCruiser;
		private Camera _camera;
		private ICameraCalculator _cameraCalculator;
		private float _playerCruiserOrthographicSize;
		private float _aiCruiserOrthographicSize;
		private Vector3 _playerCruiserTargetPosition;
		private Vector3 _aiCruiserTargetPosition;
		private Vector3 _centerTargetPosition;

		private CameraState _cameraStateTarget;
		private Vector3 _cameraVelocity = Vector3.zero;
		private Vector3 _cameraPositionTarget;

		private float _cameraOrthographicSizeChangeVelocity = 0;
		private float _cameraOrthographicSizeTarget;

		public float smoothTime;
		public float overviewOrthographicSize;
		public float centerPositionY;

		public event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		public event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;

		private CameraState _cameraState;
		public CameraState State { get { return _cameraState; } }

		public void Initialise(ICruiser playerCruiser, ICruiser aiCruiser)
		{
			_playerCruiser = playerCruiser;
			_aiCruiser = aiCruiser;
			_cameraState = CameraState.Center;
			_cameraStateTarget = CameraState.PlayerCruiser;

			_camera = GetComponent<Camera>();
			Assert.IsNotNull(_camera);

			_cameraCalculator = new CameraCalculator(_camera);

			_playerCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(_playerCruiser);
			_playerCruiserTargetPosition = new Vector3(_playerCruiser.Position.x, _cameraCalculator.FindCameraYPosition(_playerCruiserOrthographicSize), transform.position.z);

			_aiCruiserOrthographicSize = _cameraCalculator.FindCameraOrthographicSize(_aiCruiser);
			_aiCruiserTargetPosition = new Vector3(_aiCruiser.Position.x, _cameraCalculator.FindCameraYPosition(_aiCruiserOrthographicSize), transform.position.z);

			_centerTargetPosition = new Vector3(0, centerPositionY, transform.position.z);

			FocusOnPlayerCruiser();
		}

		void Update()
		{
			if (_cameraState != _cameraStateTarget)
			{
				// Camera position
				bool isInPosition = (transform.position - _cameraPositionTarget).magnitude < POSITION_EQUALITY_MARGIN;
				if (!isInPosition)
				{
					transform.position = Vector3.SmoothDamp(transform.position, _cameraPositionTarget, ref _cameraVelocity, smoothTime);
				}
				else if (transform.position != _cameraPositionTarget)
				{
					transform.position = _cameraPositionTarget;
					Logging.Log(Tags.CAMERA_CONTROLLER, "CameraController position done");
				}

				// Camera zoom
				bool isRightOrthographicSize = Math.Abs(Camera.main.orthographicSize - _cameraOrthographicSizeTarget) < ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN;
				if (!isRightOrthographicSize)
				{
					Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, _cameraOrthographicSizeTarget, ref _cameraOrthographicSizeChangeVelocity, smoothTime);
				}
				else if (Camera.main.orthographicSize != _cameraOrthographicSizeTarget)
				{
					Camera.main.orthographicSize = _cameraOrthographicSizeTarget;
					Logging.Log(Tags.CAMERA_CONTROLLER, "CameraController zoom done");
				}

				// Camera state
				if (isInPosition && isRightOrthographicSize)
				{
					if (CameraTransitionCompleted != null)
					{
						CameraTransitionCompleted.Invoke(this, new CameraTransitionArgs(_cameraState, _cameraStateTarget));
					}

					_cameraState = _cameraStateTarget;
				}
			}
		}

		public void FocusOnPlayerCruiser()
		{
			bool isInstant = _cameraState != CameraState.Center;
			MoveCamera(CameraState.PlayerCruiser, isInstant);
		}

		public void FocusOnAiCruiser()
		{
			bool isInstant = _cameraState != CameraState.Center;
			MoveCamera(CameraState.AiCruiser, isInstant);
		}

		public void ShowFullMapView()
		{
			MoveCamera(CameraState.Center, isInstant: false);
		}

		/// <returns><c>true</c>, if camera was or will be moved, <c>false</c> otherwise.</returns>
		private bool MoveCamera(CameraState targetState, bool isInstant)
		{
			bool willMoveCamera = 
				_cameraState != CameraState.InTransition
			    && _cameraState != targetState;

			Logging.Log(Tags.CAMERA_CONTROLLER, "MoveCamera targetState: " + targetState + "  willMoveCamera: " + willMoveCamera + "  _cameraState: " + _cameraState);

			if (willMoveCamera)
			{
				_cameraPositionTarget = transform.position;
				_cameraStateTarget = targetState;

				if (CameraTransitionStarted != null)
				{
					CameraTransitionStarted.Invoke(this, new CameraTransitionArgs(_cameraState, _cameraStateTarget));
				}

				switch (targetState)
				{
					case CameraState.AiCruiser:
						_cameraPositionTarget = _aiCruiserTargetPosition;
						_cameraOrthographicSizeTarget = _aiCruiserOrthographicSize;
						break;

					case CameraState.PlayerCruiser:
						_cameraPositionTarget = _playerCruiserTargetPosition;
						_cameraOrthographicSizeTarget = _playerCruiserOrthographicSize;
						break;

					case CameraState.Center:
						_cameraPositionTarget = _centerTargetPosition;
						_cameraOrthographicSizeTarget = overviewOrthographicSize;
						break;

					default:
						throw new ArgumentException();
				}

				if (isInstant)
				{
					transform.position = _cameraPositionTarget;
					Camera.main.orthographicSize = _cameraOrthographicSizeTarget;
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
