using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Defensive
{
	public class SamSiteController : DefensiveTurret
	{
		public override void StaticInitialise()
		{
			base.StaticInitialise();
			_attackCapabilities.Add(TargetType.Aircraft);
		}

		protected override void InitialiseTurretBarrel()
		{
			SamSiteBarrelController barrelController = _barrelController as SamSiteBarrelController;
			Assert.IsNotNull(barrelController);

			IExactMatchTargetFilter targetFilter = _targetsFactory.CreateExactMatchTargetFiler();
			barrelController.Initialise(targetFilter, CreateAngleCalculator(), CreateRotationMovementController(), _movementControllerFactory, _targetPositionPredictorFactory);
		}

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
