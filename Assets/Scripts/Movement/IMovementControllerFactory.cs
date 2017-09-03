using System.Collections.Generic;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using UnityEngine;

namespace BattleCruisers.Movement
{
    public interface IMovementControllerFactory
	{
        // Velocity
		IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<IPatrolPoint> patrolPoints);
		IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS);
        FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS);
		IMovementController CreateDummyMovementController();

        // Velcotiy => Homing
		IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, SafeZone safeZone);
		IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, float cruisingAltitudeInM, IFlightPointsProvider flightPointsProvider);
        IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider);

        // Rotation
		IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform);
		IRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true);
		IConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform);
		IConstantRotationController CreateDummyConstantRotationController();
	}
}
