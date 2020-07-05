using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public class SmoothPositionAdjuster : ISmoothPositionAdjuster
    {
		private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly float _smoothTime;
		private Vector3 _cameraPositionChangeVelocity;
        
		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;
		private const float MAX_SPEED = 1000;

        public SmoothPositionAdjuster(ICamera camera, ITime time, float smoothTime)
		{
            Helper.AssertIsNotNull(camera, time);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_camera = camera;
            _time = time;
			_smoothTime = smoothTime;
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
                        _smoothTime, 
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
