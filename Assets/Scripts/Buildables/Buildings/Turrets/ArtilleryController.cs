using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	// FELIX  Avoid duplication with AntiAirTurretController
	public class ArtilleryController : OffensiveTurret
	{
		private ShellTurretBarrelController _barrelController;

		protected override TurretBarrelController BarrelController { get { return _barrelController; } }

		protected override void OnAwake()
		{
			base.OnAwake();

			_barrelController = gameObject.GetComponentInChildren<ShellTurretBarrelController>();
			Assert.IsNotNull(_barrelController);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateArtilleryAngleCalcultor(_targetPositionPredictorFactory);
			_barrelController.Initialise(Faction, angleCalculator);
		}
	}
}
