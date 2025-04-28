using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Projectiles.Stats;
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
    public class RocketController :
        ProjectileWithTrail<ProjectileActivationArgs, ProjectileStats>,
        ITargetProvider
    {
        private RocketTarget _rocketTarget;
        public GameObject rocketSprite; //for making more complicated rocket sprites disappear on detonation

        public ITarget Target { get; private set; }

        public override void Initialise()
        {
            base.Initialise();

            _rocketTarget = GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(_rocketTarget);

            if (rocketSprite != null)
            {
                rocketSprite.SetActive(true); // Enable the sprite on initialization
            }
        }

        public override void Activate(ProjectileActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.Target;

            IVelocityProvider maxVelocityProvider = new StaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;
            IFlightPointsProvider flightPointsProvider
                = activationArgs.ProjectileStats.IsAccurate ?
                    FactoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider :
                    FactoryProvider.FlightPointsProviderFactory.InaccurateRocketFlightPointsProvider;

            MovementController
                = new RocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    activationArgs.ProjectileStats.CruisingAltitudeInM,
                    flightPointsProvider);

            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(activationArgs.Parent.Faction, _rigidBody, this);

            if (rocketSprite != null)
            {
                rocketSprite.SetActive(true);
            }
        }

        protected override void OnImpactCleanUp()
        {
            base.OnImpactCleanUp();

            if (rocketSprite != null)
            {
                rocketSprite.SetActive(false);
            }

            _rocketTarget.GameObject.SetActive(false);
        }
    }
}