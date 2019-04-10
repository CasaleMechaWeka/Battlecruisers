using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public class SmoothZoomAdjuster : ISmoothZoomAdjuster
	{
		private readonly ICamera _camera;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly float _smoothTime;
		private float _cameraOrthographicSizeChangeVelocity;
        
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;
        private const float MAX_SPEED = 1000;

        public SmoothZoomAdjuster(ICamera camera, IDeltaTimeProvider deltaTimeProvider, float smoothTime)
		{
            Helper.AssertIsNotNull(camera, deltaTimeProvider);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_camera = camera;
            _deltaTimeProvider = deltaTimeProvider;
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
                        _deltaTimeProvider.UnscaledDeltaTime);
            }
            else
            {
                _camera.OrthographicSize = targetOrthographicSize;
            }

            return isRightOrthographicSize;
		}
	}
}
