using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
	public class MortarTargetPositionPredictor : TargetPositionPredictor
	{
		protected override float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPosition, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			float sourceElevationInM = sourcePosition.y - targetPosition.y;
			float vSin = projectileVelocityInMPerS * Mathf.Sin(currentAngleInRadians);
			float squareRootArg = (vSin * vSin) + 2 * Constants.GRAVITY * sourceElevationInM;
			return (vSin + Mathf.Sqrt(squareRootArg)) / Constants.GRAVITY;
		}
	}
}

