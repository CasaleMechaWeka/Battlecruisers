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

		private CameraState _state;
        public CameraState State 
		{ 
			get { return _state; }
            private set
			{
				if (StateChanged != null)
                {
                    StateChanged.Invoke(this, new CameraStateChangedArgs(_state, value));
                }

				_state = value;
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

			_state = CameraState.Overview;
		}

        public bool SetCameraTarget(CameraState targetState)
        {
			bool willMoveCamera =
                State != CameraState.InTransition
                && State != targetState;

            if (willMoveCamera)
            {
				Assert.IsTrue(_stateToTarget.ContainsKey(targetState));
				_target = _stateToTarget[targetState];

                if (_target.IsInstantTransition(State))
                {
                    // Move camera instantly
                    _camera.Position = _target.Position;
                    _camera.OrthographicSize = _target.OrthographicSize;
                }
				
                State = CameraState.InTransition;
            }

            return willMoveCamera;
        }

		public void MoveCamera(float deltaTime, CameraState currentState)
		{
			// FELIX  Want to throw in these conditions?  Only want this called if we're in a transition :P
			if (_target == null || State == _target.State)
			{
				// Camera is already in the right place.  No need to move the camera.
				return;
			}

            bool isInPosition = _positionAdjuster.AdjustPosition(_target.Position);
			bool isRightOrthographicSize = _zoomAdjuster.AdjustZoom(_target.OrthographicSize);

            if (isInPosition && isRightOrthographicSize)
            {
				State = _target.State;
            }
		}
	}
}
