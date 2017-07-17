using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
	// FELIX  Avoid duplciate code with RocketController?
	public class NukeController : ProjectileController, ITargetProvider
	{
		public ITarget Target { get; private set; }

		public void Initialise(RocketStats rocketStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, ITarget target, 
			IMovementControllerFactory movementControllerFactory, IFlightPointsProvider flightPointsProvider)
		{
			base.Initialise(rocketStats, initialVelocityInMPerS, targetFilter);

			Target = target;

			_movementController = movementControllerFactory.CreateRocketMovementController(_rigidBody, rocketStats.MaxVelocityInMPerS, this, rocketStats.CruisingAltitudeInM, flightPointsProvider);
		}
	}
}