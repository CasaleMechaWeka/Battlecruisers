using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Movement
{
    public class MovementControllerFactory : IMovementControllerFactory
	{
        private readonly IRotationHelper _rotationHelper;

        public const float DEFAULT_POSITION_EQUALITY_MARGIN_IN_M = 0.5f;

		public MovementControllerFactory()
		{
            _rotationHelper = new RotationHelper();
		}

        #region Velocity
        #region Homing
        public IMovementController CreateMissileMovementController(
            Rigidbody2D rigidBody, 
            IVelocityProvider maxVelocityProvider, 
			ITargetProvider targetProvider, 
            ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MissileMovementController(rigidBody, maxVelocityProvider, targetProvider, targetPositionPredictorFactory);
		}

		public IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, Rectangle safeZone)
		{
			return new FighterMovementController(rigidBody, maxVelocityProvider, targetProvider, safeZone);
		}

		public IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, float cruisingAltitudeInM, IFlightPointsProvider flightPointsProvider)
		{
			return new RocketMovementController(rigidBody, maxVelocityProvider, targetProvider, cruisingAltitudeInM, flightPointsProvider);
		}

        public IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider)
        {
            return new HomingMovementController(rigidBody, maxVelocityProvider, targetProvider);
        }
		#endregion Homing

		#region Providers
		public IVelocityProvider CreateStaticVelocityProvider(float velocityInMPerS)
		{
            return new StaticVelocityProvider(velocityInMPerS);
		}

		public IVelocityProvider CreateMultiplyingVelocityProvider(IVelocityProvider providerToWrap, float multiplier)
		{
            return new MultiplyingVelocityProvider(providerToWrap, multiplier);
		}

        public IVelocityProvider CreatePatrollingVelocityProvider(IPatrollingVelocityProvider patrollingAircraft)
        {
            return new PatrollingVelocityProvider(patrollingAircraft);
        }
        #endregion Providers

        public IMovementController CreatePatrollingMovementController(
            Rigidbody2D rigidBody, 
            IVelocityProvider maxVelocityProvider, 
            IList<IPatrolPoint> patrolPoints, 
            float positionEqualityMarginInM = DEFAULT_POSITION_EQUALITY_MARGIN_IN_M)
		{
            return new PatrollingMovementController(rigidBody, maxVelocityProvider, patrolPoints, positionEqualityMarginInM);
		}

		public IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider)
		{
			return new BomberMovementController(rigidBody, maxVelocityProvider);
		}

		public FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider)
		{
            return new FollowingXAxisMovementController(rigidBody, maxVelocityProvider);
		}

		public IMovementController CreateDummyMovementController()
		{
			return new DummyMovementController();
		}
        #endregion Velocity

        #region Rotation
        public IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform, IDeltaTimeProvider deltaTimeProvider)
		{
            return new RotationMovementController(_rotationHelper, new TransformBC(transform), deltaTimeProvider, rotateSpeedInDegreesPerS);
		}

		public IRotationMovementController CreateDummyRotationMovementController(bool isOnTarget = true)
		{
			return new DummyRotationMovementController(isOnTarget);
		}

		public IConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
		{
			return new ConstantRotationController(rotateSpeedInDegreesPerS, transform);
		}

		public IConstantRotationController CreateDummyConstantRotationController()
		{
			return new DummyConstantRotationController();
		}
		#endregion Rotation

        public IMovementDecider CreateShipMovementDecider(
            IShip ship, 
            IBroadcastingTargetProvider blockingEnemyTargetProvider, 
            IBroadcastingTargetProvider blockingFriendTargetProvider,
            ITargetTracker inRangeTargetTracker,
            ITargetTracker shipBlockerTargetTracker,
            ITargetRangeHelper rangeHelper)
        {
            return new ShipMovementDecider(ship, blockingEnemyTargetProvider, blockingFriendTargetProvider, inRangeTargetTracker, shipBlockerTargetTracker, rangeHelper);
        }
    }
}
