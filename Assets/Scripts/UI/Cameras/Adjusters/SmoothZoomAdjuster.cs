using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
	public class SmoothZoomAdjuster : ISmoothZoomAdjuster
	{
		private readonly ICamera _camera;
		private readonly float _smoothTime;
		private float _cameraOrthographicSizeChangeVelocity;
        
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;

        // FELIX  Use ICamera
		public SmoothZoomAdjuster(ICamera camera, float smoothTime)
		{
			Assert.IsNotNull(camera);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_camera = camera;
			_smoothTime = smoothTime;
			_cameraOrthographicSizeChangeVelocity = 0;
		}

		public bool AdjustZoom(float targetOrthographicSize)
		{
			bool isRightOrthographicSize = Mathf.Abs(_camera.OrthographicSize - targetOrthographicSize) < ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN;

            if (!isRightOrthographicSize)
            {
				_camera.OrthographicSize = Mathf.SmoothDamp(_camera.OrthographicSize, targetOrthographicSize, ref _cameraOrthographicSizeChangeVelocity, _smoothTime);
            }
            else
            {
                _camera.OrthographicSize = targetOrthographicSize;
            }

            return isRightOrthographicSize;
		}
	}
}
