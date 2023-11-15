using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity
{
    /// <summary>
    /// Moves along the x-axis until we are on the same y-axis as the target.
    /// </summary>
    public class PvPFollowingXAxisMovementController : PvPTargetVelocityMovementController, IPvPTargetConsumer
    {
        public IPvPTarget Target { private get; set; }

        public PvPFollowingXAxisMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider)
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
