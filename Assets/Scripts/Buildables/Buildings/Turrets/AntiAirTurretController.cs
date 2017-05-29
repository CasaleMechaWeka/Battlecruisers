using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class AntiAirTurretController : DefensiveTurret
	{
		protected override void OnInitialised()
		{
			base.OnInitialised();

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateLeadingAngleCalcultor(_targetPositionPredictorFactory);
			turretBarrelController.Initialise(Faction, angleCalculator);
		}
	}
}

