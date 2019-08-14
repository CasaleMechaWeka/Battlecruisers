using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;
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
    public class RocketController : ProjectileControllerBase<ICruisingProjectileStats>, ITargetProvider
	{
		public ITarget Target { get; private set; }

		public void Initialise(
            ICruisingProjectileStats rocketStats,
            Vector2 initialVelocityInMPerS,
            ITargetFilter targetFilter,
            ITarget target,
            IFactoryProvider factoryProvider, 
            ITarget parent,
            Faction faction)
		{
            base.Initialise(rocketStats, initialVelocityInMPerS, targetFilter, factoryProvider, parent);

			Target = target;

            IVelocityProvider maxVelocityProvider = factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(rocketStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

			MovementController 
                = factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider, 
                    targetProvider, 
                    rocketStats.CruisingAltitudeInM, 
                    factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider);

			RocketTarget rocketTarget = gameObject.GetComponentInChildren<RocketTarget>();
			Assert.IsNotNull(rocketTarget);

            rocketTarget.Initialise(faction, _rigidBody);
		}
	}
}