using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class MortarController : DefensiveTurret
	{
		protected override IAngleCalculator CreateAngleCalculator(IAngleCalculatorFactory angleCalculatorFactory)
		{
			return angleCalculatorFactory.CreateMortarAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}

