using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public abstract class PvPTargetPositionPredictor : IPvPTargetPositionPredictor
    {
        public Vector2 PredictTargetPosition(Vector2 sourcePosition, Vector2 targetPositionToAttack, IPvPTarget target, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            float timeToTargetEstimate = EstimateTimeToTarget(sourcePosition, targetPositionToAttack, projectileVelocityInMPerS, currentAngleInRadians);

            float projectedX = targetPositionToAttack.x + target.Velocity.x * timeToTargetEstimate;
            float projectedY = targetPositionToAttack.y + target.Velocity.y * timeToTargetEstimate;

            Vector2 projectedPosition = new Vector2(projectedX, projectedY);
            Logging.Verbose(Tags.PREDICTORS, $"target: {target}  projectedPosition: {projectedPosition}  targetVelocity: {target.Velocity}  timeToTargetEstimate: {timeToTargetEstimate}");
            return projectedPosition;
        }

        protected virtual float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPositionToAttack, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            return 0;
        }
    }
}

