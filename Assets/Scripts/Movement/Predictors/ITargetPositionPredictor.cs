using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public interface ITargetPositionPredictor
    {
        Vector2 PredictTargetPosition(Vector2 sourcePosition, Vector2 targetPositionToAttack, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians);
    }
}