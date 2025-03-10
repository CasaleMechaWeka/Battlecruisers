using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public class SmoothPositionAdjuster : ISmoothPositionAdjuster
    {
		private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly ICameraSmoothTimeProvider _smoothTimeProvider;
		private Vector3 _cameraPositionChangeVelocity;
        
		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;
		private const float MAX_SPEED = 1000;

        public SmoothPositionAdjuster(ICamera camera, ITime time, ICameraSmoothTimeProvider smoothTimeProvider)
		{
            Helper.AssertIsNotNull(camera, time, smoothTimeProvider);

			_camera = camera;
            _time = time;
			_smoothTimeProvider = smoothTimeProvider;
			_cameraPositionChangeVelocity = Vector3.zero;
		}

        public bool AdjustPosition(Vector3 targetPosition)
		{
			bool isInPosition = (_camera.Position - targetPosition).magnitude < POSITION_EQUALITY_MARGIN;

            if (!isInPosition)
            {
                _camera.Position 
                    = Vector3.SmoothDamp(
                        _camera.Position, 
                        targetPosition, 
                        ref _cameraPositionChangeVelocity, 
                        _smoothTimeProvider.SmoothTime, 
                        MAX_SPEED, 
                        _time.UnscaledDeltaTime);
            }
            else
            {
                _camera.Position = targetPosition;
            }

            Logging.Verbose(Tags.CAMERA, $"target: {targetPosition}  Actual: {_camera.Position}");

            return isInPosition;
		}
    }
}
