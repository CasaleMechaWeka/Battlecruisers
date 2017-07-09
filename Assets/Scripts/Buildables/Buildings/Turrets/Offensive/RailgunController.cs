using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Offensive
{
	public class RailgunController : OffensiveTurret
	{
		// FELIX  Fix
		protected override void InitialiseTurretBarrel()
		{
//			RocketBarrelController barrelController = _barrelController as RocketBarrelController;
//			Assert.IsNotNull(barrelController);
//			barrelController.Initialise(CreateTargetFilter(), CreateAngleCalculator(), _movementControllerFactory, Faction);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
