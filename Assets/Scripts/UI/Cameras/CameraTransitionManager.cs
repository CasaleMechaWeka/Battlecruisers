using System;
using System.Collections.Generic;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
	public class CameraTransitionManager : ICameraTransitionManager
    {
		private readonly ICamera _camera;
		private readonly ISmoothPositionAdjuster _positionAdjuster;
		private readonly ISmoothZoomAdjuster _zoomAdjuster;
		private readonly IDictionary<CameraState, ICameraTarget> _stateToTarget;

		private ICameraTarget _target;
		public CameraState TargetState 
		{ 
			set
			{
				Assert.IsTrue(_stateToTarget.ContainsKey(value));
                _target = _stateToTarget[value];
			}
		}
        
		private CameraState _state;
        public CameraState State 
		{ 
			get { return _state; }
            private set
            {
				// Event handlers may access this property, so want to update the 
                // value before emitting the changed event.
				CameraState oldState = _state;
				_state = value;

                if (oldState != _state && StateChanged != null)
                {
					Logging.Log(Tags.CAMERA, "CameraTransitionManager.State: " + oldState + " > " + value);
					StateChanged.Invoke(this, new CameraStateChangedArgs(oldState, _state));
                }
			}
		}

		public event EventHandler<CameraStateChangedArgs> StateChanged;

		public CameraTransitionManager(
			ICamera camera, 
			ICameraTargetsFactory cameraTargetsFactory,
			ISmoothPositionAdjuster positionAdjuster,
			ISmoothZoomAdjuster zoomAdjuster)
		{
			Helper.AssertIsNotNull(camera, cameraTargetsFactory, positionAdjuster, zoomAdjuster);

			_camera = camera;
			_stateToTarget = cameraTargetsFactory.CreateCameraTargets();
			_positionAdjuster = positionAdjuster;
			_zoomAdjuster = zoomAdjuster;

			_state = CameraState.PlayerInputControlled;
		}

		public void MoveCamera(float deltaTime, CameraState currentState)
		{
			Assert.IsNotNull(_target);

            if (State == _target.State)
            {
                // Already in the right place, fake completed transition
				State = CameraState.InTransition;
				State = _target.State;
			}

			// Not in right place.  Need to move camera
			State = CameraState.InTransition;

			if (_target.IsInstantTransition(State))
			{
				// Move camera instantly
				_camera.Position = _target.Position;
				_camera.OrthographicSize = _target.OrthographicSize;
			}
			else
			{
				// Move camera smoothly over several frames
				bool isInPosition = _positionAdjuster.AdjustPosition(_target.Position);
				bool isRightOrthographicSize = _zoomAdjuster.AdjustZoom(_target.OrthographicSize);

				if (isInPosition && isRightOrthographicSize)
				{
					State = _target.State;
				}
			}
		}

		public void Reset(CameraState currentState)
		{
			Logging.Log(Tags.CAMERA, "CameraTransitionManager.Reset(): " + currentState);
			_state = currentState;
		}
	}
}
