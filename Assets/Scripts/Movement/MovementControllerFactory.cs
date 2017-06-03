using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using System;
using UnityEngine;

namespace BattleCruisers.Movement
{
	public interface IMovementControllerFactory
	{
		IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IHomingMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone);
		IHomingMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, float cruisingAltitudeInM);
	}

	public class MovementControllerFactory : IMovementControllerFactory
	{
		public IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MissileMovementController(rigidBody, maxVelocityInMPerS, targetPositionPredictorFactory);
		}

		public IHomingMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone)
		{
			return new FighterMovementController(rigidBody, maxVelocityInMPerS, safeZone);
		}

		public IHomingMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, float cruisingAltitudeInM)
		{
			return new RocketMovementController(rigidBody, maxVelocityInMPerS, cruisingAltitudeInM);
		}
	}
}

