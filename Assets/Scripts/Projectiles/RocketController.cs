using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
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
        ProjectileWithTrail<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, 
        ITargetProvider
	{
        private RocketTarget _rocketTarget;

		public ITarget Target { get; private set; }

        public override void Initialise(ILocTable commonStrings, IFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            _rocketTarget = GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(_rocketTarget);
        }

        public override void Activate(TargetProviderActivationArgs<ICruisingProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.Target;

            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;
            // FELIX
            //IFlightPointsProvider flightPointsProvider = 
            //activationArgs.ProjectileStats.

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    activationArgs.ProjectileStats.CruisingAltitudeInM,
                    // FELIX Choose depending on accuracy
                    _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider);

            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(_commonStrings, activationArgs.Parent.Faction, _rigidBody, this);
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rocketTarget.GameObject.SetActive(false);
        }
    }
}