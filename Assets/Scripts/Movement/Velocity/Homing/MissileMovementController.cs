using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity.Homing
{
	public class MissileMovementController : HomingMovementController
	{
		private ITargetPositionPredictor _targetPositionPredictor;

		public MissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory)
			: base(rigidBody, maxVelocityInMPerS, targetProvider) 
		{ 
			_targetPositionPredictor = targetPositionPredictorFactory.CreateLinearPredictor();
		}

		protected override Vector2 FindTargetPosition()
		{
			return _targetPositionPredictor.PredictTargetPosition(_rigidBody.transform.position, _targetProvider.Target, _maxVelocityInMPerS, currentAngleInRadians: -1);
		}

		protected override float FindVelocitySmoothTime(Vector2 targetPosition)
		{
			float distance = Vector2.Distance(_rigidBody.transform.position, targetPosition);
			float smoothTimeInS = distance / _maxVelocityInMPerS;
			if (smoothTimeInS > MAX_VELOCITY_SMOOTH_TIME)
			{
				smoothTimeInS = MAX_VELOCITY_SMOOTH_TIME;
			}

			Logging.Log(Tags.MOVEMENT, "smoothTimeInS: " + smoothTimeInS);
			return smoothTimeInS;
		}
	}
}

