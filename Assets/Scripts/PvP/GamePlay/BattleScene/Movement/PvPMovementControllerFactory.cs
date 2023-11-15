using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Homing;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement
{
    public class PvPMovementControllerFactory : IPvPMovementControllerFactory
    {
        private readonly IPvPRotationHelper _rotationHelper;

        public const float DEFAULT_POSITION_EQUALITY_MARGIN_IN_M = 0.5f;

        public PvPMovementControllerFactory()
        {
            _rotationHelper = new PvPRotationHelper();
        }

        #region Velocity
        #region Homing
        public IPvPMovementController CreateMissileMovementController(
            Rigidbody2D rigidBody,
            IPvPVelocityProvider maxVelocityProvider,
            IPvPTargetProvider targetProvider,
            IPvPTargetPositionPredictorFactory targetPositionPredictorFactory)
        {
            return new PvPMissileMovementController(rigidBody, maxVelocityProvider, targetProvider, targetPositionPredictorFactory);
        }

        public IPvPMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider, PvPRectangle safeZone)
        {
            return new PvPFighterMovementController(rigidBody, maxVelocityProvider, targetProvider, safeZone);
        }

        public IPvPMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider, float cruisingAltitudeInM, IPvPFlightPointsProvider flightPointsProvider)
        {
            return new PvPRocketMovementController(rigidBody, maxVelocityProvider, targetProvider, cruisingAltitudeInM, flightPointsProvider);
        }

        public IPvPMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider, IPvPTargetProvider targetProvider)
        {
            return new PvPHomingMovementController(rigidBody, maxVelocityProvider, targetProvider);
        }
        #endregion Homing

        #region Providers
        public IPvPVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS)
        {
            return new PvPStaticVelocityProvider(velocityInMPerS);
        }

        public IPvPVelocityProvider CreateMultiplyingVelocityProvider(IPvPVelocityProvider providerToWrap, float multiplier)
        {
            return new PvPMultiplyingVelocityProvider(providerToWrap, multiplier);
        }

        public IPvPVelocityProvider CreatePatrollingVelocityProvider(IPvPPatrollingVelocityProvider patrollingAircraft)
        {
            return new PvPPatrollingVelocityProvider(patrollingAircraft);
        }
        #endregion Providers

        public IPvPMovementController CreatePatrollingMovementController(
            Rigidbody2D rigidBody,
            IPvPVelocityProvider maxVelocityProvider,
            IList<IPvPPatrolPoint> patrolPoints,
            float positionEqualityMarginInM = DEFAULT_POSITION_EQUALITY_MARGIN_IN_M)
        {
            return new PvPPatrollingMovementController(rigidBody, maxVelocityProvider, patrolPoints, positionEqualityMarginInM);
        }

        public IPvPBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider)
        {
            return new PvPBomberMovementController(rigidBody, maxVelocityProvider);
        }

        public PvPFollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IPvPVelocityProvider maxVelocityProvider)
        {
            return new PvPFollowingXAxisMovementController(rigidBody, maxVelocityProvider);
        }

        public IPvPMovementController CreateDummyMovementController()
        {
            return new PvPDummyMovementController();
        }
        #endregion Velocity

        #region Rotation
        public IPvPRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform, IPvPDeltaTimeProvider deltaTimeProvider)
        {
            return new PvPRotationMovementController(_rotationHelper, new PvPTransformBC(transform), deltaTimeProvider, rotateSpeedInDegreesPerS);
        }

        public IPvPRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true)
        {
            return new PvPDummyRotationMovementController(isOnTarget);
        }

        public IPvPConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
        {
            return new PvPConstantRotationController(rotateSpeedInDegreesPerS, transform);
        }

        public IPvPConstantRotationController CreateDummyConstantRotationController()
        {
            return new PvPDummyConstantRotationController();
        }
        #endregion Rotation

        public IPvPMovementDecider CreateShipMovementDecider(
            IPvPShip ship,
            IPvPBroadcastingTargetProvider blockingEnemyTargetProvider,
            IPvPBroadcastingTargetProvider blockingFriendTargetProvider,
            IPvPTargetTracker inRangeTargetTracker,
            IPvPTargetTracker shipBlockerTargetTracker,
            IPvPTargetRangeHelper rangeHelper)
        {
            return new PvPShipMovementDecider(ship, blockingEnemyTargetProvider, blockingFriendTargetProvider, inRangeTargetTracker, shipBlockerTargetTracker, rangeHelper);
        }
    }
}
