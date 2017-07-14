using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
	public class BomberMovementController : IBomberMovementController
	{
		private readonly Rigidbody2D _rigidBody;
		private readonly float _maxVelocityInMPerS;

		private float _velocitySmoothTime;
		private Vector2 _velocity;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;

		public Vector2 Velocity
		{
			get { return _rigidBody.velocity; }
			set { _rigidBody.velocity = value; }
		}

		private Vector2 _targetVelocity;
		public Vector2 TargetVelocity
		{
			get { return _targetVelocity; }
			set
			{
				_targetVelocity = value;
				float velocityChange = (_rigidBody.velocity - _targetVelocity).magnitude;
				_velocitySmoothTime = velocityChange / _maxVelocityInMPerS;
			}
		}

		public event EventHandler<XDirectionChangeEventArgs> DirectionChanged;

		public BomberMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			Assert.IsNotNull(rigidBody);
			Assert.IsTrue(maxVelocityInMPerS > 0);

			_rigidBody = rigidBody;
			_maxVelocityInMPerS = maxVelocityInMPerS;
		}

		public void AdjustVelocity()
		{ 
			if (_rigidBody.velocity != TargetVelocity)
			{
				Vector2 oldVelocity = _rigidBody.velocity;

				if ((_rigidBody.velocity - TargetVelocity).magnitude <= VELOCITY_EQUALITY_MARGIN)
				{
					_rigidBody.velocity = TargetVelocity;
				}
				else
				{
					_rigidBody.velocity = Vector2.SmoothDamp(_rigidBody.velocity, TargetVelocity, ref _velocity, _velocitySmoothTime, _maxVelocityInMPerS, Time.deltaTime);
				}

				// FELIX
				//			UpdateFacingDirection(oldVelocity, rigidBody.velocity);
			}
		}
	}
}
