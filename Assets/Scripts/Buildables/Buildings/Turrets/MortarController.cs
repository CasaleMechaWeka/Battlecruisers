using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class MortarController : DefensiveTurret
	{
		protected override void OnInitialised()
		{
			base.OnInitialised();

			IAngleCalculator angleCalculator = _angleCalculatorFactory.CreateMortarAngleCalcultor(_targetPositionPredictorFactory);
			_turretBarrelController.Initialise(Faction, angleCalculator);
		}
	}
}

