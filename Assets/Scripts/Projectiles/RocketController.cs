using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
	public class RocketController : ProjectileController
	{
		private ITarget _target;
		private IHomingMovementController _movementController;

		public void Initialise(RocketStats rocketStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, ITarget target, IMovementControllerFactory movementControllerFactory)
		{
			base.Initialise(rocketStats, initialVelocityInMPerS, targetFilter);

			_target = target;

			_movementController = movementControllerFactory.CreateRocketMovementController(_rigidBody, rocketStats.MaxVelocityInMPerS, rocketStats.CruisingAltitudeInM);
			_movementController.Target = _target;
		}

		// FELIX  Common with MissileController?  Worth extracting?
		void FixedUpdate()
		{
			_movementController.AdjustVelocity();

			// Adjust game object to point in direction it's travelling
			transform.right = _rigidBody.velocity;
		}
	}
}