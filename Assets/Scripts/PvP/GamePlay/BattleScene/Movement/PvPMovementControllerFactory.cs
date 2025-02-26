using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Homing;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System.Collections.Generic;
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
        public IMovementController CreateMissileMovementController(
            Rigidbody2D rigidBody,
            IVelocityProvider maxVelocityProvider,
            ITargetProvider targetProvider,
            ITargetPositionPredictorFactory targetPositionPredictorFactory)
        {
            return new PvPMissileMovementController(rigidBody, maxVelocityProvider, targetProvider, targetPositionPredictorFactory);
        }

        public IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, Rectangle safeZone)
        {
            return new PvPFighterMovementController(rigidBody, maxVelocityProvider, targetProvider, safeZone);
        }

        public IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, float cruisingAltitudeInM, IFlightPointsProvider flightPointsProvider)
        {
            return new PvPRocketMovementController(rigidBody, maxVelocityProvider, targetProvider, cruisingAltitudeInM, flightPointsProvider);
        }

        public IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider)
        {
            return new PvPHomingMovementController(rigidBody, maxVelocityProvider, targetProvider);
        }
        #endregion Homing

        #region Providers
        public IVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS)
        {
            return new PvPStaticVelocityProvider(velocityInMPerS);
        }

        public IVelocityProvider CreateMultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier)
        {
            return new PvPMultiplyingVelocityProvider(providerToWrap, multiplier);
        }

        public IVelocityProvider CreatePatrollingVelocityProvider(IPatrollingVelocityProvider patrollingAircraft)
        {
            return new PvPPatrollingVelocityProvider(patrollingAircraft);
        }
        #endregion Providers

        public IMovementController CreatePatrollingMovementController(
            Rigidbody2D rigidBody,
            IVelocityProvider maxVelocityProvider,
            IList<IPatrolPoint> patrolPoints,
            float positionEqualityMarginInM = DEFAULT_POSITION_EQUALITY_MARGIN_IN_M)
        {
            return new PvPPatrollingMovementController(rigidBody, maxVelocityProvider, patrolPoints, positionEqualityMarginInM);
        }

        public IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider)
        {
            return new PvPBomberMovementController(rigidBody, maxVelocityProvider);
        }

        public FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider)
        {
            return new FollowingXAxisMovementController(rigidBody, maxVelocityProvider);
        }

        public IMovementController CreateDummyMovementController()
        {
            return new PvPDummyMovementController();
        }
        #endregion Velocity

        #region Rotation
        public IPvPRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform, IDeltaTimeProvider deltaTimeProvider)
        {
            return new PvPRotationMovementController(_rotationHelper, new TransformBC(transform), deltaTimeProvider, rotateSpeedInDegreesPerS);
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
            IBroadcastingTargetProvider blockingEnemyTargetProvider,
            IBroadcastingTargetProvider blockingFriendTargetProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetTracker shipBlockerTargetTracker,
            ITargetRangeHelper rangeHelper)
        {
            return new PvPShipMovementDecider(ship, blockingEnemyTargetProvider, blockingFriendTargetProvider, inRangeTargetTracker, shipBlockerTargetTracker, rangeHelper);
        }
    }
}
