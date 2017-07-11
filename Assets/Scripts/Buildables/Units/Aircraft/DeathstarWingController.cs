using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
	// FELIX  Avoid duplicate code with TurretBarrelController
	public class DeathstarWingController : MonoBehaviour 
	{
		private IAngleCalculator _angleCalculator;
		private float _desiredAngleInDegrees;
		private float _rotateSpeedInDegreesPerS;

		private const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

		public void Initialise(IAngleCalculator angleCalculator, float rotateSpeedInDegreesPerS)
		{
			_angleCalculator = angleCalculator;
			_rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
			_desiredAngleInDegrees = transform.rotation.eulerAngles.z;
		}

		public void StartRotatingWing(float desiredAngleInDegrees)
		{
			_desiredAngleInDegrees = desiredAngleInDegrees;
		}

		void FixedUpdate()
		{
			if (!IsOnTarget(_desiredAngleInDegrees))
			{
				AdjustRotation(_desiredAngleInDegrees);
			}
		}

		private bool IsOnTarget(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = transform.rotation.eulerAngles.z;
			float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			return differenceInDegrees < ROTATION_EQUALITY_MARGIN_IN_DEGREES;
		}

		private void AdjustRotation(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = transform.rotation.eulerAngles.z;
			float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			float directionMultiplier = _angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);

			float rotationIncrement = Time.deltaTime * _rotateSpeedInDegreesPerS;
			if (rotationIncrement > differenceInDegrees)
			{
				rotationIncrement = differenceInDegrees;
			}
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;

			transform.Rotate(rotationIncrementVector);
		}
	}
}
