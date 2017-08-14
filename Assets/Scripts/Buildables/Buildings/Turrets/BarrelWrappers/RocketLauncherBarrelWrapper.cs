using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class RocketLauncherBarrelWrapper : StaticBarrelWrapper
    {
        protected override float DesiredAngleInDegrees { get { return 60; } }

        protected override void InitialiseBarrelController()
        {
            RocketBarrelController barrelController = _barrelController as RocketBarrelController;
            Assert.IsNotNull(barrelController);

            barrelController
                .Initialise(
                    CreateTargetFilter(),
                    CreateAngleCalculator(),
                    CreateRotationMovementController(),
                    _factoryProvider.MovementControllerFactory,
                    _enemyFaction,
                    _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider);
        }
    }
}
