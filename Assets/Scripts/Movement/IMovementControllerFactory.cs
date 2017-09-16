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
		IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, IList<IPatrolPoint> patrolPoints);
		IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
		IMovementController CreateDummyMovementController();

        // Velocity => Homing
		IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, SafeZone safeZone);
		IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, float cruisingAltitudeInM, IFlightPointsProvider flightPointsProvider);
        IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider);

        // Velocity => Providers
        IVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS);

        // Rotation
		IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform);
		IRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true);
		IConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform);
		IConstantRotationController CreateDummyConstantRotationController();
	}
}
