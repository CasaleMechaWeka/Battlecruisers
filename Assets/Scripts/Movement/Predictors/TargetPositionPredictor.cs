using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public abstract class TargetPositionPredictor : ITargetPositionPredictor
	{
		public Vector2 PredictTargetPosition(Vector2 sourcePosition, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			float timeToTargetEstimate = EstimateTimeToTarget(sourcePosition, target.Position, projectileVelocityInMPerS, currentAngleInRadians);

			float projectedX = target.Position.x + target.Velocity.x * timeToTargetEstimate;
			float projectedY = target.Position.y + target.Velocity.y * timeToTargetEstimate;

			Vector2 projectedPosition = new Vector2(projectedX, projectedY);
			Logging.Log(Tags.PREDICTORS, string.Format("target: {0}  projectedPosition: {1}  targetVelocity: {2}  timeToTargetEstimate: {3}", target, projectedPosition, target.Velocity, timeToTargetEstimate));
			return projectedPosition;
		}

		protected virtual float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPosition, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			return 0;
		}
	}
}

