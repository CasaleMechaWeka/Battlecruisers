using BattleCruisers.Buildables;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils;

namespace BattleCruisers.Movement.Velocity.Homing
{
	public class HomingMovementController : IHomingMovementController
	{
		protected readonly Rigidbody2D _rigidBody;
		protected readonly float _maxVelocityInMPerS;
		private Vector2 _velocity;

		private ITarget _target;
		public ITarget Target 
		{ 
			protected get { return _target; }
			set
			{
				_target = value;
				OnTargetSet();
			}
		}

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
		protected const float MAX_VELOCITY_SMOOTH_TIME = 1;

		public HomingMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			Assert.IsNotNull(rigidBody);
			Assert.IsTrue(maxVelocityInMPerS > 0);

			_rigidBody = rigidBody;
			_maxVelocityInMPerS = maxVelocityInMPerS;
		}

		public void AdjustVelocity()
		{
			Assert.IsTrue(Target != null);

			Vector2 sourcePosition = _rigidBody.transform.position;
			Vector2 targetPosition = FindTargetPosition();
			Vector2 desiredVelocity = FindDesiredVelocity(sourcePosition, targetPosition, _maxVelocityInMPerS);

			if (Math.Abs(_rigidBody.velocity.x - desiredVelocity.x) <= VELOCITY_EQUALITY_MARGIN
				&& Math.Abs(_rigidBody.velocity.y - desiredVelocity.y) <= VELOCITY_EQUALITY_MARGIN)
			{
				_rigidBody.velocity = desiredVelocity;
			}
			else
			{
				float velocitySmoothTime = FindVelocitySmoothTime(targetPosition);

				Logging.Log(Tags.MOVEMENT, string.Format("AdjustVelocity():  _rigidBody.velocity: {0}  desiredVelocity: {1}  _velocitySmoothTime: {2}  maxVelocityInMPerS: {3}", 
					_rigidBody.velocity, desiredVelocity, velocitySmoothTime, _maxVelocityInMPerS));

				_rigidBody.velocity = Vector2.SmoothDamp(_rigidBody.velocity, desiredVelocity, ref _velocity, velocitySmoothTime, _maxVelocityInMPerS, Time.deltaTime);
			}
		}

		protected virtual Vector2 FindTargetPosition()
		{
			return Target.GameObject.transform.position;
		}

		private Vector2 FindDesiredVelocity(Vector2 sourcePosition, Vector2 targetPosition, float maxVelocityInMPerS)
		{
			Vector2 desiredVelocity = new Vector2(0, 0);

			if (sourcePosition == targetPosition)
			{
				return desiredVelocity;
			}

			if (sourcePosition.x == targetPosition.x)
			{
				// On same x-axis
				desiredVelocity.y = sourcePosition.y < targetPosition.y ? maxVelocityInMPerS : -maxVelocityInMPerS;
			}
			else if (sourcePosition.y == targetPosition.y)
			{
				// On same y-axis
				desiredVelocity.x = sourcePosition.x < targetPosition.x ? maxVelocityInMPerS : -maxVelocityInMPerS;
			}
			else
			{
				// Different x and y axes, so need to calculate the angle
				float xDiff = Math.Abs(sourcePosition.x - targetPosition.x);
				float yDiff = Math.Abs(sourcePosition.y - targetPosition.y);
				float angleInRadians = Mathf.Atan(yDiff / xDiff);
				float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

				float velocityX = Mathf.Cos(angleInRadians) * maxVelocityInMPerS;
				float velocityY = Mathf.Sin(angleInRadians) * maxVelocityInMPerS;
				Logging.Log(Tags.MOVEMENT, string.Format("FighterController.FindDesiredVelocity()  angleInDegrees: {0}  velocityX: {1}  velocityY: {2}",
					angleInDegrees, velocityX, velocityY));

				if (sourcePosition.x > targetPosition.x)
				{
					// Source is to right of target
					velocityX *= -1;
				}

				if (sourcePosition.y > targetPosition.y)
				{
					// Source is above target
					velocityY *= -1;
				}

				desiredVelocity.x = velocityX;
				desiredVelocity.y = velocityY;
			}

			Logging.Log(Tags.MOVEMENT, "FighterController.FindDesiredVelocity() " + desiredVelocity);
			return desiredVelocity;
		}
	
		protected virtual float FindVelocitySmoothTime(Vector2 targetPosition)
		{
			return MAX_VELOCITY_SMOOTH_TIME;
		}

		protected virtual void OnTargetSet() { }
	}
}
