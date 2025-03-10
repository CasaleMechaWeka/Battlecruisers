using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
    public class BomberMovementController : TargetVelocityMovementController, IBomberMovementController
	{
		private float _velocitySmoothTime;

		private Vector2 _targetVelocity;
		public Vector2 TargetVelocity
		{
			get { return _targetVelocity; }
			set
			{
                Logging.Log(Tags.MOVEMENT, $"{_targetVelocity} > {value}");
				_targetVelocity = value;
				float velocityChange = (_rigidBody.velocity - _targetVelocity).magnitude;
                _velocitySmoothTime = velocityChange / _maxVelocityProvider.VelocityInMPerS;
			}
		}

		public BomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider)
            : base(rigidBody, maxVelocityProvider) { }

        protected override Vector2 FindDesiredVelocity()
        {
            return TargetVelocity;
        }

        protected override float FindVelocitySmoothTime()
        {
            return _velocitySmoothTime;
        }
    }
}
