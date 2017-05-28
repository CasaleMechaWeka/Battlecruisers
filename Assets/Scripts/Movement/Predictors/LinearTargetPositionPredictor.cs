using System;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
	public class LinearTargetPositionPredictor : TargetPositionPredictor
	{
		protected override float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPosition, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			return Vector2.Distance(sourcePosition, targetPosition) / projectileVelocityInMPerS;
		}
	}
}

