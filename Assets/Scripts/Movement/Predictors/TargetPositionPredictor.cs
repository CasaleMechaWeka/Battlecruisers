using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
	public interface ITargetPositionPredictor
	{
		Vector2 PredictTargetPosition(Vector2 sourcePosition, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians);
	}

	public abstract class TargetPositionPredictor
	{
		public Vector2 PredictTargetPosition(Vector2 sourcePosition, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians)
		{
			Vector2 targetPosition = target.GameObject.transform.position;

			float timeToTargetEstimate = EstimateTimeToTarget(sourcePosition, targetPosition, projectileVelocityInMPerS, currentAngleInRadians);

			float projectedX = targetPosition.x + target.Velocity.x * timeToTargetEstimate;
			float projectedY = targetPosition.y + target.Velocity.y * timeToTargetEstimate;

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

