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

        public CameraState CurrentState { private set; get; }

		public event EventHandler<CameraTransitionArgs> CameraTransitionStarted;
		public event EventHandler<CameraTransitionArgs> CameraTransitionCompleted;

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

			CurrentState = CameraState.Overview;
		}

        public bool SetCameraTarget(CameraState targetState)
        {
			bool willMoveCamera =
                CurrentState != CameraState.InTransition
                && CurrentState != targetState;

            if (willMoveCamera)
            {
				Assert.IsTrue(_stateToTarget.ContainsKey(targetState));
				_target = _stateToTarget[targetState];

                if (CameraTransitionStarted != null)
                {
                    CameraTransitionStarted.Invoke(this, new CameraTransitionArgs(CurrentState, targetState));
                }

                if (_target.IsInstantTransition(CurrentState))
                {
					// Move camera instantly
					_camera.Position = _target.Position;
                    _camera.OrthographicSize = _target.OrthographicSize;
                }
                else
                {
					// Move camera via smooth transition, over multiple frames.
                    CurrentState = CameraState.InTransition;
                }
            }

            return willMoveCamera;
        }

		public void MoveCamera()
		{
			if (_target == null || CurrentState == _target.State)
			{
				// Camera is already in the right place.  No need to move the camera.
				return;
			}

            bool isInPosition = _positionAdjuster.AdjustPosition(_target.Position);
			bool isRightOrthographicSize = _zoomAdjuster.AdjustZoom(_target.OrthographicSize);

            if (isInPosition && isRightOrthographicSize)
            {
                if (CameraTransitionCompleted != null)
                {
                    CameraTransitionCompleted.Invoke(this, new CameraTransitionArgs(CurrentState, _target.State));
                }

				CurrentState = _target.State;
            }
		}
	}
}
