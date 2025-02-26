using BattleCruisers.Buildables;
using BattleCruisers.Movement.Predictors;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors
{
    public class PvPDummyTargetPositionpredictor : ITargetPositionPredictor
    {
        public Vector2 PredictTargetPosition(Vector2 sourcePosition, Vector2 targetPositionToAttack, ITarget target, float projectileVelocityInMPerS, float currentAngleInRadians)
        {
            return targetPositionToAttack;
        }
    }
}