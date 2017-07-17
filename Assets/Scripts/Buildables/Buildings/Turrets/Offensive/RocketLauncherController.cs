using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Offensive
{
	public class RocketLauncherController : OffensiveTurret
	{
		private const float ROCKET_LAUNCH_ANGLE_IN_DEGREES = 60;

		protected override void InitialiseTurretBarrel()
		{
			// FELIX TEMP  Create factory :)
			IFlightPointsProvider flightPointsProvider = new RocketFlightPointsProvider();


			RocketBarrelController barrelController = _barrelController as RocketBarrelController;
			Assert.IsNotNull(barrelController);
			barrelController.Initialise(CreateTargetFilter(), CreateAngleCalculator(), CreateRotationMovementController(), _movementControllerFactory, Faction, flightPointsProvider);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateStaticAngleCalculator(_targetPositionPredictorFactory, ROCKET_LAUNCH_ANGLE_IN_DEGREES);
		}
	}
}
