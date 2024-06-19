using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public abstract class TargetPositionPredictor : ITargetPositionPredictor
    {
        public Vector2 PredictTargetPosition(Vector2 sourcePosition, Vector2 currentTargetPosition, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            float timeToTargetEstimate = EstimateTimeToTarget(sourcePosition, currentTargetPosition, projectileVelocityInMPerS, currentAngleInRadians, target);

            float projectedX = currentTargetPosition.x + target.Velocity.x * timeToTargetEstimate;
            float projectedY = currentTargetPosition.y + target.Velocity.y * timeToTargetEstimate;

            Vector2 projectedPosition = new Vector2(projectedX, projectedY);
            Logging.Verbose(Tags.PREDICTORS, $"target: {target}  projectedPosition: {projectedPosition}  targetVelocity: {target.Velocity}  timeToTargetEstimate: {timeToTargetEstimate}");
            return projectedPosition;
        }

        protected virtual float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPositionToAttack, float projectileVelocityInMPerS, float currentAngleInRadians, ITarget target)
        {
            return 0;
        }
    }
}

