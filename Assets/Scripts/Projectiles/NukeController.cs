using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;

namespace BattleCruisers.Projectiles
{
    public class NukeController : ProjectileController, ITargetProvider
	{
		private IMovementControllerFactory _movementControllerFactory;
		private INukeStats _nukeStats;
		private IFlightPointsProvider _flightPointsProvider;

		public ITarget Target { get; private set; }

		public void Initialise(
            INukeStats nukeStats, 
            ITargetFilter targetFilter, 
            IDamageApplier damageApplier, 
            ITarget target, 
			IMovementControllerFactory movementControllerFactory, 
            IFlightPointsProvider flightPointsProvider)
		{
            base.Initialise(nukeStats, nukeStats.InitialVelocity, targetFilter, damageApplier);

			_movementControllerFactory = movementControllerFactory;
			_nukeStats = nukeStats;
			_flightPointsProvider = flightPointsProvider;

			Target = target;
		}

		public void Launch()
		{
            IVelocityProvider maxVelocityProvider = _movementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			_movementController 
                = _movementControllerFactory.CreateRocketMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    _nukeStats.CruisingAltitudeInM, 
                    _flightPointsProvider);
		}
	}
}
