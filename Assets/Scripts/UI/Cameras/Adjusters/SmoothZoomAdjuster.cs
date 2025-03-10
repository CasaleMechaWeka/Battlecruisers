using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public class SmoothZoomAdjuster : ISmoothZoomAdjuster
	{
		private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly ICameraSmoothTimeProvider _smoothTimeProvider;
		private float _cameraOrthographicSizeChangeVelocity;
        
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
        private const float MAX_SPEED = 1000;

        public SmoothZoomAdjuster(ICamera camera, ITime time, ICameraSmoothTimeProvider smoothTimeProvider)
		{
            Helper.AssertIsNotNull(camera, time, smoothTimeProvider);

			_camera = camera;
            _time = time;
			_smoothTimeProvider = smoothTimeProvider;
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
                        _smoothTimeProvider.SmoothTime, 
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
