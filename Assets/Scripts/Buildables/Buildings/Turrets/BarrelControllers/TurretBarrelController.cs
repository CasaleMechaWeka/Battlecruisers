using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
	public class TurretBarrelController : BarrelController 
	{
		private const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

		protected override bool IsOnTarget(float desiredAngleInDegrees)
		{
			float currentAngleInDegrees = transform.rotation.eulerAngles.z;
			float differenceInDegrees = Math.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			return differenceInDegrees < ROTATION_EQUALITY_MARGIN_IN_DEGREES;
		}

		protected override void AdjustBarrel(float desiredAngleInDegrees)
		{

			float currentAngleInDegrees = transform.rotation.eulerAngles.z;
			float differenceInDegrees = Math.Abs(currentAngleInDegrees - desiredAngleInDegrees);
			float directionMultiplier = angleCalculator.FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);
			Logging.Log(Tags.BARREL_CONTROLLER, $"MoveBarrelToAngle():  currentAngleInDegrees: {currentAngleInDegrees}  desiredAngleInDegrees: {desiredAngleInDegrees}  directionMultiplier: {directionMultiplier}");

			float rotationIncrement = Time.deltaTime * turretStats.turretRotateSpeedInDegrees;
			if (rotationIncrement > differenceInDegrees)
			{
				rotationIncrement = differenceInDegrees;
			}
			Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;
			Logging.Log(Tags.BARREL_CONTROLLER, $"rotationIncrement: {rotationIncrement}");

			transform.Rotate(rotationIncrementVector);
		}
	}
}
