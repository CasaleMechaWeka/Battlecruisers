using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Projectiles
{
    public class NukeController : ProjectileControllerBase<INukeStats>, ITargetProvider
	{
		private IMovementControllerFactory _movementControllerFactory;
		private INukeStats _nukeStats;
		private IFlightPointsProvider _flightPointsProvider;

		public ITarget Target { get; private set; }

		public void Initialise(
            INukeStats nukeStats,
            ITargetFilter targetFilter,
            ITarget target,
            IFactoryProvider factoryProvider,
            ITarget parent)
		{
            base.Initialise(nukeStats, nukeStats.InitialVelocity, targetFilter, factoryProvider, parent);

            _movementControllerFactory = factoryProvider.MovementControllerFactory;
			_nukeStats = nukeStats;
            _flightPointsProvider = factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;

			Target = target;
		}

		public void Launch()
		{
            IVelocityProvider maxVelocityProvider = _movementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			MovementController 
                = _movementControllerFactory.CreateRocketMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    _nukeStats.CruisingAltitudeInM, 
                    _flightPointsProvider);
		}
	}
}
