using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public class DummyTargetPositionpredictor : ITargetPositionPredictor
    {
        public Vector2 PredictTargetPosition(Vector2 sourcePosition, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            return target.Position;
        }
    }
}