using BattleCruisers.Movement.Velocity.Providers;
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

		private const float VELOCITY_EQUALITY_MARGIN = 0.1f;
		protected const float MAX_VELOCITY_SMOOTH_TIME = 1;

		public sealed override Vector2 Velocity
		{
			get { return _rigidBody.velocity; }
			set { _rigidBody.velocity = value; }
		}

        public TargetVelocityMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider)
            : base(maxVelocityProvider)
		{
			Assert.IsNotNull(rigidBody);
			_rigidBody = rigidBody;
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
                Vector2 oldVelocity = Velocity;

                float velocitySmoothTime = FindVelocitySmoothTime();
                _rigidBody.velocity 
                    = Vector2.SmoothDamp(
                        _rigidBody.velocity, 
                        desiredVelocity, 
                        ref _velocity, 
                        velocitySmoothTime, 
                        _maxVelocityProvider.VelocityInMPerS,
                        _time.DeltaTime);

                HandleDirectionChange(oldVelocity, Velocity);
            }
        }

        protected abstract Vector2 FindDesiredVelocity();

        protected virtual float FindVelocitySmoothTime()
        {
            return MAX_VELOCITY_SMOOTH_TIME;
        }
    }
}
