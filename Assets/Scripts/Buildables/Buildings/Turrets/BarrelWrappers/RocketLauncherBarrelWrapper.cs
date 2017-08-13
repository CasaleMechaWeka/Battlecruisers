using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
	public class RocketLauncherBarrelWrapper : BarrelWrapper
	{
		private const float ROCKET_LAUNCH_ANGLE_IN_DEGREES = 60;

		protected override void InitialiseBarrelController()
		{
			RocketBarrelController barrelController = _barrelController as RocketBarrelController;
			Assert.IsNotNull(barrelController);

			barrelController.Initialise(CreateTargetFilter(), CreateAngleCalculator(), CreateRotationMovementController(),
                _factoryProvider.MovementControllerFactory, _enemyFaction, _factoryProvider.FlightPointsProviderFactory.RocketFlightPointsProvider);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateStaticAngleCalculator(_factoryProvider.TargetPositionPredictorFactory, ROCKET_LAUNCH_ANGLE_IN_DEGREES);
		}
	}
}
