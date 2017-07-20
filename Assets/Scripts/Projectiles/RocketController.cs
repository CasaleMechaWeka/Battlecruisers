using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
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
    public class RocketController : ProjectileController, ITargetProvider
	{
		public ITarget Target { get; private set; }

		public void Initialise(RocketStats rocketStats, Vector2 initialVelocityInMPerS, ITargetFilter targetFilter, ITarget target, 
			IMovementControllerFactory movementControllerFactory, Faction faction, IFlightPointsProvider flightPointsProvider)
		{
			base.Initialise(rocketStats, initialVelocityInMPerS, targetFilter);

			Target = target;

			_movementController = movementControllerFactory.CreateRocketMovementController(_rigidBody, rocketStats.MaxVelocityInMPerS, this, rocketStats.CruisingAltitudeInM, flightPointsProvider);

			RocketTarget rocketTarget = gameObject.GetComponentInChildren<RocketTarget>();
			Assert.IsNotNull(rocketTarget);
			rocketTarget.Initialise(faction, _rigidBody);
		}
	}
}