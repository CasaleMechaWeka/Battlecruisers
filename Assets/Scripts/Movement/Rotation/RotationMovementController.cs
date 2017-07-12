using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement
{
	// FELIX   Move to own class
	public interface IRotationMovementController
	{
		bool IsOnTarget(float desiredAngleInDegrees);
		void AdjustRotation(float desiredAngleInDegrees);
	}

	// FELIX  Try RigidBody.MoveRotation
	public class RotationMovementController : IRotationMovementController
	{
		private readonly float _rotateSpeedInDegreesPerS;
		private readonly IAngleCalculator _angleCalculator;
		private readonly Transform _transform;

		private const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

		public RotationMovementController(IAngleCalculator angleCalculator, float rotateSpeedInDegreesPerS, Transform transform)
		{
			Assert.IsNotNull(angleCalculator);
			Assert.IsTrue(rotateSpeedInDegreesPerS > 0);

			_angleCalculator = angleCalculator;
			_rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
		}

		public bool IsOnTarget(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = _transform.rotation.eulerAngles.z;
			float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			return differenceInDegrees < ROTATION_EQUALITY_MARGIN_IN_DEGREES;
		}

		public void AdjustRotation(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = _transform.rotation.eulerAngles.z;
			float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);

			float rotationIncrement = Time.deltaTime * _rotateSpeedInDegreesPerS;
			if (rotationIncrement > differenceInDegrees)
			{
				rotationIncrement = differenceInDegrees;
			}
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;

			_transform.Rotate(rotationIncrementVector);
		}
	}
}