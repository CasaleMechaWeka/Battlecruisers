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
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
	/// <summary>
	/// The RocketController wants the behaviour of both:
	/// 1. ProjectileController
	/// 2. Target
	/// But it can only subclass one of these.  Hence subclass ProjectileController, and
	/// have a child game object deriving of Target, to get both behaviours.
	/// </summary>
	public class RocketController : ProjectileController
	{
		private ITarget _target;

		public void Initialise(RocketStats rocketStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, ITarget target, 
			IMovementControllerFactory movementControllerFactory, Faction faction)
		{
			base.Initialise(rocketStats, initialVelocityInMPerS, targetFilter);

			_target = target;

			_movementController = movementControllerFactory.CreateRocketMovementController(_rigidBody, rocketStats.MaxVelocityInMPerS, rocketStats.CruisingAltitudeInM);
			_movementController.Target = _target;

			RocketTarget rocketTarget = gameObject.GetComponentInChildren<RocketTarget>();
			Assert.IsNotNull(rocketTarget);
			rocketTarget.Initialise(faction, _rigidBody);
		}
	}
}