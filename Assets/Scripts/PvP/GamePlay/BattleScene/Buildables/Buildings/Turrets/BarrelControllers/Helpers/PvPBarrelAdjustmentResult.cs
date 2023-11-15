using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    // PERF  Struct candidate :)
    public class PvPBarrelAdjustmentResult
    {
        public bool IsOnTarget { get; }
        public float DesiredAngleInDegrees { get; }
        public Vector2 PredictedTargetPosition { get; }

        public PvPBarrelAdjustmentResult(
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
            PvPBarrelAdjustmentResult other = obj as PvPBarrelAdjustmentResult;
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
