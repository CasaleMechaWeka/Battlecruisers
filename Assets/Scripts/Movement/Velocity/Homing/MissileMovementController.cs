using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity.Homing
{
    public class MissileMovementController : HomingMovementController
	{
		private ITargetPositionPredictor _targetPositionPredictor;

		public MissileMovementController(
            Rigidbody2D rigidBody, 
            IVelocityProvider maxVelocityProvider, 
            ITargetProvider targetProvider, 
            ITargetPositionPredictorFactory targetPositionPredictorFactory)
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
