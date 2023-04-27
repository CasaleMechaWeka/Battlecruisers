using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement
{
    public interface IPvPMovementControllerFactory
    {
        // Velocity
        IPvPMovementController CreatePatrollingMovementController(
            Rigidbody2D rigidBody,
            IPvPVelocityProvider maxVelocityProvider,
            IList<IPvPPatrolPoint> patrolPoints,
            float positionEqualityMarginInM = PvPMovementControllerFactory.DEFAULT_POSITION_EQUALITY_MARGIN_IN_M);
        IPvPBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider);
        PvPFollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider);
        IPvPMovementController CreateDummyMovementController();

        // Velocity => Homing
        IPvPMovementController CreateMissileMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider, IPvPTargetPositionPredictorFactory targetPositionPredictorFactory);
        IPvPMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider, PvPRectangle safeZone);
        IPvPMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider, float cruisingAltitudeInM, IPvPFlightPointsProvider flightPointsProvider);
        IPvPMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider);

        // Velocity => Providers
        IPvPVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS);
        IPvPVelocityProvider CreateMultiplyingVelocityProvider(IPvPVelocityProvider providerToWrap, float multiplier);
        IPvPVelocityProvider CreatePatrollingVelocityProvider(IPvPPatrollingVelocityProvider patrollingVelocityProvider);

        // Rotation
        IPvPRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform, IPvPDeltaTimeProvider deltaTimeProvider);
        IPvPRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true);
        IPvPConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform);
        IPvPConstantRotationController CreateDummyConstantRotationController();

        // Deciers
        IPvPMovementDecider CreateShipMovementDecider(
            IPvPShip ship,
            IPvPBroadcastingTargetProvider blockingEnemyTargetProvider,
            IPvPBroadcastingTargetProvider blockingFriendTargetProvider,
            IPvPTargetTracker inRangeTargetTracker,
            IPvPTargetTracker shipBlockerTargetTracker,
            IPvPTargetRangeHelper rangeHelper);
    }
}
