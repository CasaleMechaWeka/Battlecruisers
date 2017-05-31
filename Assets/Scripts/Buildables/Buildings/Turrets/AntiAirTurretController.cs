using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class AntiAirTurretController : DefensiveTurret
	{
		protected override IAngleCalculator CreateAngleCalculator(IAngleCalculatorFactory angleCalculatorFactory)
		{
			return angleCalculatorFactory.CreateLeadingAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}

