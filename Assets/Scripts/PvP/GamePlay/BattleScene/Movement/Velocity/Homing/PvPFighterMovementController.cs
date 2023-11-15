using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Homing
{
    public class PvPFighterMovementController : PvPHomingMovementController
    {
        // Zone in which fighter will pursue enemies.  If those enemies move outside this
        // safe zone the fighter will abandon pursuit.
        private readonly PvPRectangle _safeZone;

        public PvPFighterMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider, PvPRectangle safeZone)
            : base(rigidBody, maxVelocityProvider, targetProvider)
        {
            _safeZone = safeZone;
        }

        protected override Vector2 FindTargetPosition()
        {
            return CapTargetPositionInSafeZone(_targetProvider.Target.GameObject.transform.position);
        }

        private Vector2 CapTargetPositionInSafeZone(Vector2 targetPosition)
        {
            if (targetPosition.x < _safeZone.MinX)
            {
                targetPosition.x = _safeZone.MinX;
            }
            if (targetPosition.x > _safeZone.MaxX)
            {
                targetPosition.x = _safeZone.MaxX;
            }
            if (targetPosition.y < _safeZone.MinY)
            {
                targetPosition.y = _safeZone.MinY;
            }
            if (targetPosition.y > _safeZone.MaxY)
            {
                targetPosition.y = _safeZone.MaxY;
            }

            return targetPosition;
        }
    }
}
