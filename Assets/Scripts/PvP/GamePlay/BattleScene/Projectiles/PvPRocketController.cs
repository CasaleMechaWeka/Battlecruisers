using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    /// <summary>
    /// The RocketController wants the behaviour of both:
    /// 1. ProjectileController
    /// 2. Target
    /// But it can only subclass one of these.  Hence subclass ProjectileController, and
    /// have a child game object deriving of Target, to get both behaviours.
    /// </summary>
    public class PvPRocketController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>, IPvPCruisingProjectileStats>,
        IPvPTargetProvider
    {
        private PvPRocketTarget _rocketTarget;

        public IPvPTarget Target { get; private set; }

        public override void Initialise(ILocTable commonStrings, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            _rocketTarget = GetComponentInChildren<PvPRocketTarget>();
            Assert.IsNotNull(_rocketTarget);
        }

        public override void Activate(PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Target = activationArgs.Target;

            IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            IPvPTargetProvider targetProvider = this;
            IPvPFlightPointsProvider flightPointsProvider
                = activationArgs.ProjectileStats.IsAccurate ?
                    _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider :
                    _factoryProvider.FlightPointsProviderFactory.InaccurateRocketFlightPointsProvider;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateRocketMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider,
                    activationArgs.ProjectileStats.CruisingAltitudeInM,
                    flightPointsProvider);

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