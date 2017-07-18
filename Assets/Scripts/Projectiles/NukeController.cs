using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Projectiles.DamageAppliers;
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
		private IMovementControllerFactory _movementControllerFactory;
		private NukeStats _nukeStats;
		private IFlightPointsProvider _flightPointsProvider;

		public ITarget Target { get; private set; }

		public void Initialise(NukeStats nukeStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, IDamageApplier damageApplier, ITarget target, 
			IMovementControllerFactory movementControllerFactory, IFlightPointsProvider flightPointsProvider)
		{
			base.Initialise(nukeStats, initialVelocityInMPerS, targetFilter, damageApplier);

			_movementControllerFactory = movementControllerFactory;
			_nukeStats = nukeStats;
			_flightPointsProvider = flightPointsProvider;

			Target = target;
		}

		public void Launch()
		{
			_movementController = _movementControllerFactory.CreateRocketMovementController(
				_rigidBody, _nukeStats.MaxVelocityInMPerS, this, _nukeStats.CruisingAltitudeInM, _flightPointsProvider);
		}
	}
}