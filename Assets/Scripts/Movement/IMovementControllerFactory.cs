using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Movement
{
	public interface IMovementControllerFactory
	{
		IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<IPatrolPoint> patrolPoints);
		IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS);
		IMovementController CreateDummyMovementController();

		// FELIX  Return IMovementController instead?
		IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, SafeZone safeZone);
		IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, float cruisingAltitudeInM);

		IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform);
		IRotationMovementController CreateDummyRotationMovementController();
	}
}
