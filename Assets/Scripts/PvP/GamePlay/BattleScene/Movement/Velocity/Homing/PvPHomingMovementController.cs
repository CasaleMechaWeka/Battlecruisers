using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Homing
{
    public class PvPHomingMovementController : PvPTargetVelocityMovementController
    {
        protected readonly IPvPTargetProvider _targetProvider;

        public PvPHomingMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider)
            : base(rigidBody, maxVelocityProvider)
        {
            Assert.IsNotNull(targetProvider);
            _targetProvider = targetProvider;
        }

        protected override Vector2 FindDesiredVelocity()
        {
            Vector2 sourcePosition = _rigidBody.position;
            Vector2 targetPosition = FindTargetPosition();
            Vector2 desiredVelocity = new Vector2(0, 0);

            if (sourcePosition == targetPosition)
            {
                return desiredVelocity;
            }

            if (sourcePosition.x == targetPosition.x)
            {
                // On same x-axis
                desiredVelocity.y = sourcePosition.y < targetPosition.y ? _maxVelocityProvider.VelocityInMPerS : -_maxVelocityProvider.VelocityInMPerS;
            }
            else if (sourcePosition.y == targetPosition.y)
            {
                // On same y-axis
                desiredVelocity.x = sourcePosition.x < targetPosition.x ? _maxVelocityProvider.VelocityInMPerS : -_maxVelocityProvider.VelocityInMPerS;
            }
            else
            {
                // Different x and y axes, so need to calculate the angle
                float xDiff = Math.Abs(sourcePosition.x - targetPosition.x);
                float yDiff = Math.Abs(sourcePosition.y - targetPosition.y);
                float angleInRadians = Mathf.Atan(yDiff / xDiff);
                float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

                float velocityX = Mathf.Cos(angleInRadians) * _maxVelocityProvider.VelocityInMPerS;
                float velocityY = Mathf.Sin(angleInRadians) * _maxVelocityProvider.VelocityInMPerS;
                Logging.Verbose(Tags.MOVEMENT, $"angleInDegrees: {angleInDegrees}  velocityX: {velocityX}  velocityY: {velocityY}");

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

            Logging.Verbose(Tags.MOVEMENT, desiredVelocity.ToString());
            return desiredVelocity;
        }


        protected virtual Vector2 FindTargetPosition()
        {
            Assert.IsTrue(_targetProvider.Target != null);
            return _targetProvider.Target.GameObject.transform.position;
        }
    }
}
