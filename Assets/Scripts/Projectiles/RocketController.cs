using BattleCruisers.Buildables;
using BattleCruisers.Effects.Trails;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;
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
        private IProjectileTrail _trail;

		public ITarget Target { get; private set; }

        public override void Initialise(IFactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            _rocketTarget = GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(_rocketTarget);

            NukeTrailController trail = GetComponentInChildren<NukeTrailController>();
            Assert.IsNotNull(trail);
            trail.Initialise();
            _trail = trail;
        }

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

            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody, this);

            _trail.ShowAllEffects();
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();
            _rocketTarget.GameObject.SetActive(false);
            _trail.HideAliveEffects();
        }

        protected override void OnTrailsDoneCleanup()
        {
            base.OnTrailsDoneCleanup();
            _trail.HideAllEffects();
        }
    }
}