using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
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
            IList<IPvPPatrolPoint> patrolPoints,
            float positionEqualityMarginInM = PvPMovementControllerFactory.DEFAULT_POSITION_EQUALITY_MARGIN_IN_M);
        IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider);
        IMovementController CreateDummyMovementController();

        // Velocity => Homing
        IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, IPvPTargetPositionPredictorFactory targetPositionPredictorFactory);
        IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, Rectangle safeZone);
        IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, float cruisingAltitudeInM, IPvPFlightPointsProvider flightPointsProvider);
        IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider);

        // Velocity => Providers
        IVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS);
        IVelocityProvider CreateMultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier);
        IVelocityProvider CreatePatrollingVelocityProvider(IPvPPatrollingVelocityProvider patrollingVelocityProvider);

        // Rotation
        IPvPRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform, IDeltaTimeProvider deltaTimeProvider);
        IPvPRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true);
        IPvPConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform);
        IPvPConstantRotationController CreateDummyConstantRotationController();

        // Deciers
        IPvPMovementDecider CreateShipMovementDecider(
            IPvPShip ship,
            IBroadcastingTargetProvider blockingEnemyTargetProvider,
            IBroadcastingTargetProvider blockingFriendTargetProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetTracker shipBlockerTargetTracker,
            ITargetRangeHelper rangeHelper);
    }
}
