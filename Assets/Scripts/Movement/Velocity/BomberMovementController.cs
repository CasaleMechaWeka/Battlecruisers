using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
    // FELIX  Hmm.  Move finding TargetVelocity logic from BomberController to here?
    public class BomberMovementController : TargetVelocityMovementController, IBomberMovementController
	{
		private float _velocitySmoothTime;

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

		public BomberMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
            : base(rigidBody, maxVelocityInMPerS) { }

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
