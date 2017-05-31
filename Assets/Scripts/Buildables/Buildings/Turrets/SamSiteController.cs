using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class SamSiteController : DefensiveTurret
	{
		protected override IAngleCalculator CreateAngleCalculator(IAngleCalculatorFactory angleCalculatorFactory)
		{
			return angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
