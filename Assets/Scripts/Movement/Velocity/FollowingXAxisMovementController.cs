using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity
{
    /// <summary>
    /// Moves along the x-axis until we are on the same y-axis as the target.
    /// </summary>
    public class FollowingXAxisMovementController : TargetVelocityMovementController, ITargetConsumer
    {
        public ITarget Target { private get; set; }
		
        public FollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider) 
            : base(rigidBody, maxVelocityProvider) { }

        protected override Vector2 FindDesiredVelocity()
        {
            Assert.IsNotNull(Target);

            if (Target.Position.x < _rigidBody.position.x)
            {
                return new Vector2(-_maxVelocityProvider.VelocityInMPerS, 0);
            }
            else if (Target.Position.x > _rigidBody.position.x)
            {
                return new Vector2(_maxVelocityProvider.VelocityInMPerS, 0);
            }
            return new Vector2(0, 0);
        }
    }
}
