using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class SamSiteController : DefensiveTurret
	{
		private SamSiteBarrelController _barrelController;

		protected override TurretBarrelController BarrelController { get { return _barrelController; } }

		protected override void OnAwake()
		{
			base.OnAwake();

			_barrelController = gameObject.GetComponentInChildren<SamSiteBarrelController>();
			Assert.IsNotNull(_barrelController);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			IExactMatchTargetFilter targetFilter = _targetsFactory.CreateExactMatchTargetFiler();
			_barrelController.Initialise(targetFilter, CreateAngleCalculator(), _movementControllerFactory, _targetPositionPredictorFactory);
		}
		
		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
