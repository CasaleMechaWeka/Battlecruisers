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
        public IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, 
			ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MissileMovementController(rigidBody, maxVelocityInMPerS, targetProvider, targetPositionPredictorFactory);
		}

		public IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, SafeZone safeZone)
		{
			return new FighterMovementController(rigidBody, maxVelocityInMPerS, targetProvider, safeZone);
		}

		public IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, float cruisingAltitudeInM, IFlightPointsProvider flightPointsProvider)
		{
			return new RocketMovementController(rigidBody, maxVelocityInMPerS, targetProvider, cruisingAltitudeInM, flightPointsProvider);
		}

        public IMovementController CreateHomingMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider)
        {
            return new HomingMovementController(rigidBody, maxVelocityInMPerS, targetProvider);
        }
        #endregion Homing

        public IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<IPatrolPoint> patrolPoints)
		{
			return new PatrollingMovementController(rigidBody, maxPatrollilngVelocityInMPerS, patrolPoints);
		}

		public IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			return new BomberMovementController(rigidBody, maxVelocityInMPerS);
		}

		public FollowingXAxisMovementController CreateFollowingXAxisMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
            return new FollowingXAxisMovementController(rigidBody, maxVelocityInMPerS);
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
