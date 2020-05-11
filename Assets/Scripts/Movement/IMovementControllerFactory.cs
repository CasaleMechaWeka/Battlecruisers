using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Movement
{
    public interface IMovementControllerFactory
    {
        // Velocity
        IMovementController CreatePatrollingMovementController(
            Rigidbody2D rigidBody,
            IVelocityProvider maxVelocityProvider,
            IList<IPatrolPoint> patrolPoints,
            float positionEqualityMarginInM = MovementControllerFactory.DEFAULT_POSITION_EQUALITY_MARGIN_IN_M);
        IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        IMovementController CreateDummyMovementController();

        // Velocity => Homing
        IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory);
        IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, Rectangle safeZone);
        IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, float cruisingAltitudeInM, IFlightPointsProvider flightPointsProvider);
        IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider);

        // Velocity => Providers
        IVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS);
        IVelocityProvider CreateMultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier);
        IVelocityProvider CreatePatrollingVelocityProvider(IPatrollingVelocityProvider patrollingVelocityProvider);

        // Rotation
        IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform, IDeltaTimeProvider deltaTimeProvider);
        IRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true);
        IConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform);
        IConstantRotationController CreateDummyConstantRotationController();

        // Deciers
        IMovementDecider CreateShipMovementDecider(
            IShip ship, 
            IBroadcastingTargetProvider blockingEnemyTargetProvider, 
            IBroadcastingTargetProvider blockingFriendTargetProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetTracker shipBlockerTargetTracker,
            ITargetRangeHelper rangeHelper);
    }
}
