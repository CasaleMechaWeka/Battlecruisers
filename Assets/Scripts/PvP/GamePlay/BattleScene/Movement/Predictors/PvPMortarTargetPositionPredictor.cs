using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public class PvPMortarTargetPositionPredictor : PvPTargetPositionPredictor
    {
        protected override float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPositionToAttack, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            float sourceElevationInM = sourcePosition.y - targetPositionToAttack.y;
            float vSin = projectileVelocityInMPerS * Mathf.Sin(currentAngleInRadians);
            float squareRootArg = (vSin * vSin) + 2 * Constants.GRAVITY * sourceElevationInM;
            return (vSin + Mathf.Sqrt(squareRootArg)) / Constants.GRAVITY;
        }
    }
}

