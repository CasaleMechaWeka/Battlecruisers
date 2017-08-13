using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.Offensive
{
    public class RailgunController : OffensiveTurret
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
