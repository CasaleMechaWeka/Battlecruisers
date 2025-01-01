using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public interface IPvPTargetPositionPredictor
    {
        Vector2 PredictTargetPosition(Vector2 sourcePosition, Vector2 targetPositionToAttack, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians);
    }
}