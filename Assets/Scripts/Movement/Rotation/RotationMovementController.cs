using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Rotation
{
    public class RotationMovementController : IRotationMovementController
	{
		private readonly IRotationHelper _rotationHelper;
        private readonly ITransform _transform;
        private readonly ITime _time;
        private readonly float _rotateSpeedInDegreesPerS;

		public const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

        public RotationMovementController(
            IRotationHelper rotationHelper, 
            ITransform transform,
            ITime time,
            float rotateSpeedInDegreesPerS)
		{
            Helper.AssertIsNotNull(rotationHelper, transform, time);
            Assert.IsTrue(rotateSpeedInDegreesPerS > 0);

			_rotationHelper = rotationHelper;
            _transform = transform;
            _time = time;
            _rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
		}

		public bool IsOnTarget(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = _transform.EulerAngles.z;
			float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			bool isOnTarget = differenceInDegrees < ROTATION_EQUALITY_MARGIN_IN_DEGREES;

            Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, $"isOnTarget: {isOnTarget}  currentAngle: {currentAngleInDegrees}  desiredAngle: {desiredAngleInDegrees}");
            return isOnTarget;
		}

		public void AdjustRotation(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = _transform.EulerAngles.z;
			float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);

			float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
			if (rotationIncrement > differenceInDegrees)
			{
				rotationIncrement = differenceInDegrees;
			}
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;

			_transform.Rotate(rotationIncrementVector);
            Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, "Rotated transform by: " + rotationIncrement);
		}
	}
}