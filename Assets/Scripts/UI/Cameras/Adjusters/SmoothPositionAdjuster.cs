using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
	// FELIX  Use ICamera instead of transform
	public class SmoothPositionAdjuster : ISmoothPositionAdjuster
    {
		private readonly Transform _cameraTransform;
		private readonly float _smoothTime;
		private Vector3 _cameraPositionChangeVelocity;
        
		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;

		public SmoothPositionAdjuster(Transform cameraTransform, float smoothTime)
		{
			Assert.IsNotNull(cameraTransform);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_cameraTransform = cameraTransform;
			_smoothTime = smoothTime;
			_cameraPositionChangeVelocity = Vector3.zero;
		}

        public bool AdjustPosition(Vector3 targetPosition)
		{
			bool isInPosition = (_cameraTransform.position - targetPosition).magnitude < POSITION_EQUALITY_MARGIN;

            if (!isInPosition)
            {
                _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, targetPosition, ref _cameraPositionChangeVelocity, _smoothTime);
            }
            else
            {
                _cameraTransform.position = targetPosition;
            }

            return isInPosition;
		}
    }
}
