using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
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
    public class RocketController : 
        ProjectileControllerBase<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, 
        ITargetProvider
	{
		public ITarget Target { get; private set; }

        public override void Activate(TargetProviderActivationArgs<ICruisingProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.Target;

            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    activationArgs.ProjectileStats.CruisingAltitudeInM,
                    _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider);

            RocketTarget rocketTarget = gameObject.GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(rocketTarget);

            rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody);
        }
    }
}