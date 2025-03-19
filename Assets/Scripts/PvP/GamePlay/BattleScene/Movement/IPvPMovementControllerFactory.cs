using BattleCruisers.Movement.Deciders;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement
{
    public interface IPvPMovementControllerFactory
    {
        // Velocity
        IMovementController CreatePatrollingMovementController(
            Rigidbody2D rigidBody,
            IVelocityProvider maxVelocityProvider,
            IList<IPatrolPoint> patrolPoints,
            float positionEqualityMarginInM = PvPMovementControllerFactory.DEFAULT_POSITION_EQUALITY_MARGIN_IN_M);
        IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        IMovementController CreateDummyMovementController();

        // Velocity => Homing
        IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider);
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
            IPvPShip ship,
            IBroadcastingTargetProvider blockingEnemyTargetProvider,
            IBroadcastingTargetProvider blockingFriendTargetProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetTracker shipBlockerTargetTracker,
            ITargetRangeHelper rangeHelper);
    }
}
