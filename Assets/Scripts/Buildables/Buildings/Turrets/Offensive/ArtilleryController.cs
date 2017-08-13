using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.Offensive
{
    public class ArtilleryController : OffensiveTurret
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _angleCalculatorFactory.CreateArtilleryAngleCalcultor(_targetPositionPredictorFactory);
		}
	}
}
