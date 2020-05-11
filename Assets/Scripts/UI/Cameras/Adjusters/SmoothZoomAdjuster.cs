using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public class SmoothZoomAdjuster : ISmoothZoomAdjuster
	{
		private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly float _smoothTime;
		private float _cameraOrthographicSizeChangeVelocity;
        
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;
        private const float MAX_SPEED = 1000;

        public SmoothZoomAdjuster(ICamera camera, ITime time, float smoothTime)
		{
            Helper.AssertIsNotNull(camera, time);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_camera = camera;
            _time = time;
			_smoothTime = smoothTime;
			_cameraOrthographicSizeChangeVelocity = 0;
		}

		public bool AdjustZoom(float targetOrthographicSize)
		{
			bool isRightOrthographicSize = Mathf.Abs(_camera.OrthographicSize - targetOrthographicSize) < ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN;

            if (!isRightOrthographicSize)
            {
				_camera.OrthographicSize 
                    = Mathf.SmoothDamp(
                        _camera.OrthographicSize, 
                        targetOrthographicSize, 
                        ref _cameraOrthographicSizeChangeVelocity, 
                        _smoothTime, 
                        MAX_SPEED,
                        _time.UnscaledDeltaTime);
            }
            else
            {
                _camera.OrthographicSize = targetOrthographicSize;
            }

            return isRightOrthographicSize;
		}
	}
}
