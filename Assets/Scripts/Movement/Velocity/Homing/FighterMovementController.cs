using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity.Homing
{
    public class FighterMovementController : HomingMovementController
	{
		// Zone in which fighter will pursue enemies.  If those enemies move outside this
		// safe zone the fighter will abandon pursuit.
		private readonly Rectangle _safeZone;

		public FighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, Rectangle safeZone)
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
