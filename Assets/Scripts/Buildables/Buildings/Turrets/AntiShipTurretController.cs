using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class AntiShipTurretController : DefensiveTurret
	{
		protected override void OnInitialised()
		{
			base.OnInitialised();

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
			_turretBarrelController.Initialise(Faction, angleCalculator);
		}
	}
}

