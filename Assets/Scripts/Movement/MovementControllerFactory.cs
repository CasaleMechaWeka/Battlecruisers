using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.Targets;
using System;
using System.Collections.Generic;
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

		public IMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, 
			ITargetProvider targetProvider, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MissileMovementController(rigidBody, maxVelocityInMPerS, targetProvider, targetPositionPredictorFactory);
		}

		public IMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, SafeZone safeZone)
		{
			return new FighterMovementController(rigidBody, maxVelocityInMPerS, targetProvider, safeZone);
		}

		public IMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetProvider targetProvider, float cruisingAltitudeInM)
		{
			return new RocketMovementController(rigidBody, maxVelocityInMPerS, targetProvider, cruisingAltitudeInM);
		}

		public IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<IPatrolPoint> patrolPoints)
		{
			return new PatrollingMovementController(rigidBody, maxPatrollilngVelocityInMPerS, patrolPoints);
		}

		public IBomberMovementController CreateBomberMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS)
		{
			return new BomberMovementController(rigidBody, maxVelocityInMPerS);
		}

		public IMovementController CreateDummyMovementController()
		{
			return new DummyMovementController();
		}

		public IRotationMovementController CreateRotationMovementController(float rotateSpeedInDegreesPerS, Transform transform)
		{
			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictionFactory);
			return new RotationMovementController(angleCalculator, rotateSpeedInDegreesPerS, transform);
		}

		public IRotationMovementController CreateDummyRotationMovementController()
		{
			return new DummyRotationMovementController();
		}

		public IConstantRotationController CreateConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
		{
			return new ConstantRotationController(rotateSpeedInDegreesPerS, transform);
		}
	}
}
