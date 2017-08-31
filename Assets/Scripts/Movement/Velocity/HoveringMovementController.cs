using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
    /// <summary>
    /// Reduces velocity to 0, and hovers (does not move).
    /// 
    /// FELIX  Face target
    /// </summary>
    public class HoveringMovementController : TargetVelocityMovementController
	{
        private readonly Vector2 _targetVelocity;

        public HoveringMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
            : base(rigidBody, maxVelocityInMPerS)
        {
            _targetVelocity = new Vector2(0, 0);
        }

        protected override Vector2 FindDesiredVelocity()
        {
            return _targetVelocity;
        }
    }
}
