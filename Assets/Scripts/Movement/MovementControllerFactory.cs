using BattleCruisers.Movement;
using BattleCruisers.Units.Aircraft.Providers;
using System;
using UnityEngine;

namespace BattleCruisers.Movement
{
	public interface IMovementControllerFactory
	{
		IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS);
		IHomingMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone);
	}

	public class MovementControllerFactory : IMovementControllerFactory
	{
		public IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			return new MissileMovementController(rigidBody, maxVelocityInMPerS);
		}

		public IHomingMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone)
		{
			return new FighterMovementController(rigidBody, maxVelocityInMPerS, safeZone);
		}
	}
}

