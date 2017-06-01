using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
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
			Faction enemyFaction = Helper.GetOppositeFaction(Faction);
			ITargetFilter targetFilter = _targetsFactory.CreateTargetFilter(enemyFaction, _attackCapabilities);
			_barrelController.Initialise(targetFilter, angleCalculator);
		}
	}
}
