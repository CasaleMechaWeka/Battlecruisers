using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
    /// <summary>
    /// Tries to reach a target velocity.  Movement can be changed by specifying
    /// different target velocities.
    /// </summary>
    public abstract class TargetVelocityMovementController : MovementController
    {
		private Vector2 _velocity;

		protected readonly Rigidbody2D _rigidBody;
		protected readonly float _maxVelocityInMPerS;

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;

        public TargetVelocityMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			Assert.IsNotNull(rigidBody);
			Assert.IsTrue(maxVelocityInMPerS > 0);

			_rigidBody = rigidBody;
			_maxVelocityInMPerS = maxVelocityInMPerS;
		}

        public override void AdjustVelocity()
        {
            Vector2 desiredVelocity = FindDesiredVelocity();

            if (Vector2.Distance(_rigidBody.velocity, desiredVelocity) <= VELOCITY_EQUALITY_MARGIN)
            {
                _rigidBody.velocity = desiredVelocity;
            }
            else
            {
                float velocitySmoothTime = FindVelocitySmoothTime();
                _rigidBody.velocity = Vector2.SmoothDamp(_rigidBody.velocity, desiredVelocity, ref _velocity, velocitySmoothTime, _maxVelocityInMPerS, Time.deltaTime);
            }
        }

        protected abstract Vector2 FindDesiredVelocity();

        protected abstract float FindVelocitySmoothTime();
    }
}
