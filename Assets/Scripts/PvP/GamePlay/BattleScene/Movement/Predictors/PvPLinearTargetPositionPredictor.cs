using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public class PvPLinearTargetPositionPredictor : PvPTargetPositionPredictor
    {
        protected override float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPositionToAttack, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            return Vector2.Distance(sourcePosition, targetPositionToAttack) / projectileVelocityInMPerS;
        }
    }
}

