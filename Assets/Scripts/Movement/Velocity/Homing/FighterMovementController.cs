using BattleCruisers.Buildables.Units.Aircraft.Providers;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity.Homing
{
	public class FighterMovementController : HomingMovementController
	{
		// Zone in which fighter will pursue enemies.  If those enemies move outside this
		// safe zone the fighter will abandon pursuit.
		private SafeZone _safeZone;

		public FighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone)
			: base(rigidBody, maxVelocityInMPerS) 
		{ 
			_safeZone = safeZone;
		}

		protected override Vector2 FindTargetPosition()
		{
			return CapTargetPositionInSafeZone(Target.GameObject.transform.position);
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

