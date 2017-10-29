using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public interface ITargetPositionPredictor
    {
        Vector2 PredictTargetPosition(Vector2 sourcePosition, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians);
    }
}