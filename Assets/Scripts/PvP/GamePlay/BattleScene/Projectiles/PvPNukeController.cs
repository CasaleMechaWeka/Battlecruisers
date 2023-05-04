using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles
{
    public class PvPNukeController :
        PvPProjectileWithTrail<PvPTargetProviderActivationArgs<IPvPNukeStats>, IPvPNukeStats>,
        IPvPTargetProvider
    {
        private IPvPNukeStats _nukeStats;
        private IPvPFlightPointsProvider _flightPointsProvider;

        public IPvPTarget Target { get; private set; }

        public override void Activate(PvPTargetProviderActivationArgs<IPvPNukeStats> activationArgs)
        {
            base.Activate(activationArgs);

            _nukeStats = activationArgs.ProjectileStats;
            _flightPointsProvider = _factoryProvider.FlightPointsProviderFactory.NukeFlightPointsProvider;

            Target = activationArgs.Target;
        }

        public void Launch()
        {
            IPvPVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(_nukeStats.MaxVelocityInMPerS);
            IPvPTargetProvider targetProvider = this;

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
