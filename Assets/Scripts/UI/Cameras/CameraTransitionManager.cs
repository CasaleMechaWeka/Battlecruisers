using System.Collections.Generic;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras
{
	/// <summary>
    /// Handle camera transitions triggered by the navigation buttons.
    /// </summary>
	public class CameraTransitionManager : CameraMover, ICameraTransitionManager
    {
		private readonly ICamera _camera;
		private readonly ISmoothPositionAdjuster _positionAdjuster;
		private readonly ISmoothZoomAdjuster _zoomAdjuster;
		private readonly INavigationSettings _navigationSettings;
		private readonly IDictionary<CameraState, ICameraTarget> _stateToTarget;

		private ICameraTarget _target;
		public CameraState CameraTarget 
		{ 
			set
			{
				Assert.IsTrue(_stateToTarget.ContainsKey(value));
                _target = _stateToTarget[value];
			}
		}
        
		public CameraTransitionManager(
			ICamera camera, 
			ICameraTargetsFactory cameraTargetsFactory,
			ISmoothPositionAdjuster positionAdjuster,
			ISmoothZoomAdjuster zoomAdjuster,
			INavigationSettings navigationSettings)
		{
			Helper.AssertIsNotNull(camera, cameraTargetsFactory, positionAdjuster, zoomAdjuster, navigationSettings);

			_camera = camera;
			_stateToTarget = cameraTargetsFactory.CreateCameraTargets();
			_positionAdjuster = positionAdjuster;
			_zoomAdjuster = zoomAdjuster;
			_navigationSettings = navigationSettings;
		}

		public override void MoveCamera(float deltaTime)
		{
			Assert.IsNotNull(_target);

			if (!_navigationSettings.AreTransitionsEnabled)
			{
				return;
			}

			CameraState previousState = State;
			State = CameraState.InTransition;

			if (previousState == _target.State)
            {
                // Already in the right place, fake completed transition
				State = _target.State;
				return;
			}

			// Not in right place.  Need to move camera
			if (_target.IsInstantTransition(previousState))
            {
                // Move camera instantly
                _camera.Position = _target.Position;
                _camera.OrthographicSize = _target.OrthographicSize;
				State = _target.State;
				return;
			}

			// Move camera smoothly over several frames
			bool isInPosition = _positionAdjuster.AdjustPosition(_target.Position);
			bool isRightOrthographicSize = _zoomAdjuster.AdjustZoom(_target.OrthographicSize);

			if (isInPosition && isRightOrthographicSize)
			{
				State = _target.State;
			}
		}
	}
}
