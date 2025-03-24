using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement.Velocity.Homing
{
    public class HomingMovementController : TargetVelocityMovementController
    {
        protected readonly ITargetProvider _targetProvider;

        public HomingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider)
            : base(rigidBody, maxVelocityProvider)
        {
            Assert.IsNotNull(targetProvider);
            _targetProvider = targetProvider;
        }

        protected override Vector2 FindDesiredVelocity()
        {
            Vector2 targetPosition = FindTargetPosition();
            Vector2 sourcePosition = _rigidBody.position;
            if (sourcePosition == targetPosition)
                return Vector2.zero;

            float maxVelocity = _maxVelocityProvider.VelocityInMPerS;
            float dist = (sourcePosition - targetPosition).magnitude;
            //Vector2 direction = (targetPosition - sourcePosition).normalized;
            Vector2 desiredVelocity = (targetPosition - sourcePosition) / dist * maxVelocity;

            //Logging.Verbose(Tags.MOVEMENT, $"angle: {Mathf.Atan2(direction.y, direction.x)}°  velocity: {desiredVelocity}");
            return desiredVelocity;
        }

        protected virtual Vector2 FindTargetPosition()
        {
            Assert.IsTrue(_targetProvider.Target != null);
            return _targetProvider.Target.GameObject.transform.position;
        }
    }
}
