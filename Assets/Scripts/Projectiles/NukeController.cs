using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;

namespace BattleCruisers.Projectiles
{
    public class NukeController : ProjectileControllerBase<INukeStats>, ITargetProvider
	{
		private INukeStats _nukeStats;
		private IFlightPointsProvider _flightPointsProvider;

		public ITarget Target { get; private set; }

        public void Activate(TargetProviderActivationArgs<INukeStats> activationArgs)
        {
            base.Activate(activationArgs);

			_nukeStats = activationArgs.ProjectileStats;
            _flightPointsProvider = _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;

			Target = activationArgs.Target;
		}

		public void Launch()
		{
            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			MovementController 
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    _nukeStats.CruisingAltitudeInM, 
                    _flightPointsProvider);
		}
	}
}
