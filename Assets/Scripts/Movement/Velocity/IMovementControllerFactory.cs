using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using System;
using UnityEngine;

namespace BattleCruisers.Movement.Velocity
{
	public interface IMovementControllerFactory
	{
		IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IHomingMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone);
		IHomingMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, float cruisingAltitudeInM);
	}
}
