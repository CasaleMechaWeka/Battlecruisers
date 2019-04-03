using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
	public class SmoothPositionAdjuster : ISmoothPositionAdjuster
    {
		private readonly ITransform _cameraTransform;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly float _smoothTime;
		private Vector3 _cameraPositionChangeVelocity;
        
		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float MIN_SMOOTH_TIME = 0;
		private const float MAX_SPEED = 1000;

        public SmoothPositionAdjuster(ITransform cameraTransform, IDeltaTimeProvider deltaTimeProvider, float smoothTime)
		{
            Helper.AssertIsNotNull(cameraTransform, deltaTimeProvider);
			Assert.IsTrue(smoothTime > MIN_SMOOTH_TIME);

			_cameraTransform = cameraTransform;
            _deltaTimeProvider = deltaTimeProvider;
			_smoothTime = smoothTime;
			_cameraPositionChangeVelocity = Vector3.zero;
		}

        public bool AdjustPosition(Vector3 targetPosition)
		{
			bool isInPosition = (_cameraTransform.Position - targetPosition).magnitude < POSITION_EQUALITY_MARGIN;

            if (!isInPosition)
            {
                _cameraTransform.Position 
                    = Vector3.SmoothDamp(
                        _cameraTransform.Position, 
                        targetPosition, 
                        ref _cameraPositionChangeVelocity, 
                        _smoothTime, 
                        MAX_SPEED, 
                        _deltaTimeProvider.UnscaledDeltaTime);
            }
            else
            {
                _cameraTransform.Position = targetPosition;
            }

            Logging.Log(Tags.CAMERA, $"target: {targetPosition}  Actual: {_cameraTransform.Position}");

            return isInPosition;
		}
    }
}
