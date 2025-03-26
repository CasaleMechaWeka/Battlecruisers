using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Rotation
{
	public class RotationMovementController : IRotationMovementController
	{
		private readonly ITransform _transform;
		private readonly IDeltaTimeProvider _time;
		private readonly float _rotateSpeedInDegreesPerS;

		public const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

		public RotationMovementController(
			ITransform transform,
			IDeltaTimeProvider time,
			float rotateSpeedInDegreesPerS)
		{
			Helper.AssertIsNotNull(transform, time);
			Assert.IsTrue(rotateSpeedInDegreesPerS > 0);

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
			float directionMultiplier = FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);

			float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
			if (rotationIncrement > differenceInDegrees)
			{
				rotationIncrement = differenceInDegrees;
			}
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;

			Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, $"Rotation pre rotate: {_transform.EulerAngles}");
			_transform.Rotate(rotationIncrementVector);
			Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, $"Rotated transform by: {rotationIncrement}  new rotation; {_transform.EulerAngles}");
		}

		public float FindDirectionMultiplier(float currentAngleInDegrees, float desiredAngleInDegrees)
		{
			if (currentAngleInDegrees == desiredAngleInDegrees)
			{
				return 0;
			}

			float distance = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			float directionMultiplier;

			if (desiredAngleInDegrees > currentAngleInDegrees)
			{
				directionMultiplier = distance < 180 ? 1 : -1;
			}
			else
			{
				directionMultiplier = distance < 180 ? -1 : 1;
			}

			Logging.Verbose(Tags.ROTATION_HELPER, $"currentAngle: {currentAngleInDegrees}  desiredAngle: {desiredAngleInDegrees}  direction: {directionMultiplier}");
			return directionMultiplier;
		}
	}
}