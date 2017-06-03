using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class SamSiteController : DefensiveTurret
	{
		protected override void OnAwake()
		{
			base.OnAwake();
			_attackCapabilities.Add(TargetType.Aircraft);
		}

		protected override void InitialiseTurretBarrel()
		{
			SamSiteBarrelController barrelController = _barrelController as SamSiteBarrelController;
			Assert.IsNotNull(barrelController);

			IExactMatchTargetFilter targetFilter = _targetsFactory.CreateExactMatchTargetFiler();
			barrelController.Initialise(targetFilter, CreateAngleCalculator(), _movementControllerFactory, _targetPositionPredictorFactory);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
