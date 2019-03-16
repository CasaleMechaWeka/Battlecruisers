using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class BarrelAdjustmentResult
    {
        public bool IsOnTarget { get; }
        public float DesiredAngleInDegrees { get; }
        public Vector2 PredictedTargetPosition { get; }

        public BarrelAdjustmentResult(
            bool isOnTarget,
            float desiredAngleInDegrees = default,
            Vector2 predictedTargetPosition = default)
        {
            this.IsOnTarget = isOnTarget;
            this.DesiredAngleInDegrees = desiredAngleInDegrees;
            this.PredictedTargetPosition = predictedTargetPosition;
        }

        public override bool Equals(object obj)
        {
            BarrelAdjustmentResult other = obj as BarrelAdjustmentResult;
            return
                other != null
                && IsOnTarget == other.IsOnTarget
                && DesiredAngleInDegrees == other.DesiredAngleInDegrees
                && PredictedTargetPosition == other.PredictedTargetPosition;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(IsOnTarget, DesiredAngleInDegrees, PredictedTargetPosition);
        }
    }
}
