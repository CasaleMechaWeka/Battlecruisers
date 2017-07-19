using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

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