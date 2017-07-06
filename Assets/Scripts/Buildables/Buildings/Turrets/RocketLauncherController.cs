using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class RocketLauncherController : OffensiveTurret, ITargetConsumer
	{
		private const float ROCKET_LAUNCH_ANGLE_IN_DEGREES = 60;

		protected override void InitialiseTurretBarrel()
		{
			RocketBarrelController barrelController = _barrelController as RocketBarrelController;
			Assert.IsNotNull(barrelController);
			barrelController.Initialise(CreateTargetFilter(), CreateAngleCalculator(), _movementControllerFactory, Faction);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateStaticAngleCalculator(_targetPositionPredictorFactory, ROCKET_LAUNCH_ANGLE_IN_DEGREES);
		}
	}
}
