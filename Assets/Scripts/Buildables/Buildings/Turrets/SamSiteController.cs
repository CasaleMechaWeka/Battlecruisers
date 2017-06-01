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

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
			IExactMatchTargetFilter targetFilter = _targetsFactory.CreateExactMatchTargetFiler();
			_barrelController.Initialise(targetFilter, angleCalculator, _movementControllerFactory, _targetPositionPredictorFactory);
		}
	}
}
