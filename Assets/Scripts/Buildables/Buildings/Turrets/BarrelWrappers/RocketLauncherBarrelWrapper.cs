using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class RocketLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees { get { return 60; } }

        protected override void InitialiseBarrelController(BarrelController barrel)
        {
            RocketBarrelController rocketBarrel = barrel.Parse<RocketBarrelController>();

            rocketBarrel
                .Initialise(
                    CreateTargetFilter(),
                    CreateAngleCalculator(),
                    CreateRotationMovementController(barrel),
                    _factoryProvider.MovementControllerFactory,
                    _enemyFaction,
                    _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider);
        }
    }
}
