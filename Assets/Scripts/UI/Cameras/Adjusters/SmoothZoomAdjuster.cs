using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
	public class SmoothZoomAdjuster : ISmoothZoomAdjuster
	{
		private readonly Camera _camera;
		private readonly float _smoothTime;
		private float _cameraOrthographicSizeChangeVelocity;
        
		private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;

		public SmoothZoomAdjuster(Camera camera, float smoothTime)
		{
			Assert.IsNotNull(camera);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_camera = camera;
			_smoothTime = smoothTime;
		}

		public bool AdjustZoom(float targetOrthographicSize)
		{
			bool isRightOrthographicSize = Mathf.Abs(_camera.orthographicSize - targetOrthographicSize) < ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN;

            if (!isRightOrthographicSize)
            {
				_camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthographicSize, ref _cameraOrthographicSizeChangeVelocity, _smoothTime);
            }
            else if (_camera.orthographicSize != targetOrthographicSize)
            {
                _camera.orthographicSize = targetOrthographicSize;
            }

            return isRightOrthographicSize;
		}
	}
}
