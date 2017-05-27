using BattleCruisers.Movement;
using System;
using UnityEngine;

namespace BattleCruisers.Movement
{
	public interface IMovementControllerFactory
	{
		IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS);
	}

	public class MovementControllerFactory : IMovementControllerFactory
	{
		public IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			return new MissileMovementController(rigidBody, maxVelocityInMPerS);
		}
	}
}

