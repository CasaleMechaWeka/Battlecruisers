using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using UnityEngine;

namespace BattleCruisers.Movement
{
    public class MovementControllerFactory : IMovementControllerFactory
	{
		private readonly IAngleCalculatorFactory _angleCalculatorFactory;
		private readonly ITargetPositionPredictorFactory _targetPositionPredictionFactory;

		public MovementControllerFactory(IAngleCalculatorFactory angleCalculatorFactory, ITargetPositionPredictorFactory targetPositionPredictionFactory)
		{
			_angleCalculatorFactory = angleCalculatorFactory;
			_targetPositionPredictionFactory = targetPositionPredictionFactory;
		}

        #region Velocity
        #region Homing
        public IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, 
			ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MissileMovementController(rigidBody, maxVelocityProvider, targetProvider, targetPositionPredictorFactory);
		}

		public IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, ITargetProvider targetProvider, SafeZone safeZone)
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
        #endregion Providers

        public IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, IVelocityProvider maxVelocityProvider, IList<IPatrolPoint> patrolPoints)
		{
            return new PatrollingMovementController(rigidBody, maxVelocityProvider, patrolPoints);
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
        public IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform)
		{
			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictionFactory);
			return new RotationMovementController(angleCalculator, rotateSpeedInDegreesPerS, transform);
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
    }
}
