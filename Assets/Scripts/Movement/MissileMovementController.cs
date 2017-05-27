using System;
using UnityEngine;

namespace BattleCruisers.Movement
{
	public class MissileMovementController : HomingMovementController
	{
		public MissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
			: base(rigidBody, maxVelocityInMPerS) { }

		protected override Vector2 FindTargetPosition()
		{
			return PredictTargetPosition(_rigidBody.transform.position, Target.GameObject.transform.position, _maxVelocityInMPerS, Target.Velocity);
		}

		// FELIX  Avoid duplicate code with leading target in AngleCalculator
		private Vector2 PredictTargetPosition(Vector2 source, Vector2 target, float projectileVelocityInMPerS, Vector2 targetVelocity)
		{
			float timeToTargetEstimate = EstimateTimeToTarget(source, target, projectileVelocityInMPerS);

			float projectedX = target.x + targetVelocity.x * timeToTargetEstimate;
			float projectedY = target.y + targetVelocity.y * timeToTargetEstimate;

			Vector2 projectedPosition = new Vector2(projectedX, projectedY);
			// FELIX  Horror, remove once logic only in one place :P
			Debug.Log(string.Format("target: {0}  projectedPosition: {1}  targetVelocity: {2}  timeToTargetEstimate: {3}", target, projectedPosition, targetVelocity, timeToTargetEstimate));
			return projectedPosition;
		}

		private float EstimateTimeToTarget(Vector2 source, Vector2 target, float projectileVelocityInMPerS)
		{
			return Vector2.Distance(source, target) / projectileVelocityInMPerS;
		}

		protected override float FindVelocitySmoothTime(Vector2 targetPosition)
		{
			float distance = Vector2.Distance(_rigidBody.transform.position, targetPosition);
			float smoothTimeInS = distance / _maxVelocityInMPerS;
			if (smoothTimeInS > MAX_VELOCITY_SMOOTH_TIME)
			{
				smoothTimeInS = MAX_VELOCITY_SMOOTH_TIME;
			}

			Debug.Log("smoothTimeInS: " + smoothTimeInS);
			return smoothTimeInS;
		}
	}
}

