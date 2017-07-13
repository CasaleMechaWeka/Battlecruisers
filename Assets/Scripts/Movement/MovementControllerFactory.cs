using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Homing;
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

		public IHomingMovementController CreateMissileMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MissileMovementController(rigidBody, maxVelocityInMPerS, targetPositionPredictorFactory);
		}

		public IHomingMovementController CreateFighterMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, SafeZone safeZone)
		{
			return new FighterMovementController(rigidBody, maxVelocityInMPerS, safeZone);
		}

		public IHomingMovementController CreateRocketMovementController(Rigidbody2D rigidBody, float maxVelocityInMPerS, float cruisingAltitudeInM)
		{
			return new RocketMovementController(rigidBody, maxVelocityInMPerS, cruisingAltitudeInM);
		}

		public IMovementController CreatePatrollingMovementController(Rigidbody2D rigidBody, float maxPatrollilngVelocityInMPerS, IList<Vector2> patrolPoints)
		{
			return new PatrollingMovementController(rigidBody, maxPatrollilngVelocityInMPerS, patrolPoints);
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
	}
}
