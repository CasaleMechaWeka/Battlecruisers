using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Homing
{
    public class PvPMissileMovementController : PvPHomingMovementController
    {
        private IPvPTargetPositionPredictor _targetPositionPredictor;

        public PvPMissileMovementController(
            Rigidbody2D rigidBody,
            IPvPVelocityProvider maxVelocityProvider,
            IPvPTargetProvider targetProvider,
            IPvPTargetPositionPredictorFactory targetPositionPredictorFactory)
            : base(rigidBody, maxVelocityProvider, targetProvider)
        {
            _targetPositionPredictor = targetPositionPredictorFactory.CreateLinearPredictor();
        }

        protected override Vector2 FindTargetPosition()
        {
            return
                _targetPositionPredictor.PredictTargetPosition(
                    _rigidBody.position,
                    _targetProvider.Target.Position,
                    _targetProvider.Target,
                    _maxVelocityProvider.VelocityInMPerS,
                    currentAngleInRadians: -1);
        }

        protected override float FindVelocitySmoothTime()
        {
            Vector2 targetPosition = FindTargetPosition();

            float distance = Vector2.Distance(_rigidBody.position, targetPosition);
            float smoothTimeInS = distance / _maxVelocityProvider.VelocityInMPerS;
            if (smoothTimeInS > MAX_VELOCITY_SMOOTH_TIME)
            {
                smoothTimeInS = MAX_VELOCITY_SMOOTH_TIME;
            }

            Logging.Verbose(Tags.MOVEMENT, "smoothTimeInS: " + smoothTimeInS);
            return smoothTimeInS;
        }
    }
}
