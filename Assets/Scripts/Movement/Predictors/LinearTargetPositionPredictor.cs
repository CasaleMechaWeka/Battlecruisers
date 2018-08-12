using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public class LinearTargetPositionPredictor : TargetPositionPredictor
	{
		protected override float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPositionToAttack, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			return Vector2.Distance(sourcePosition, targetPositionToAttack) / projectileVelocityInMPerS;
		}
	}
}

