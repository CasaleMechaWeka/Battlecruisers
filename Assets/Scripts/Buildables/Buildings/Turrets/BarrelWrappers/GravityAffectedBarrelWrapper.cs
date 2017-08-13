using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class GravityAffectedBarrelWrapper : BarrelWrapper
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateArtilleryAngleCalcultor(_factoryProvider.TargetPositionPredictorFactory);
		}
	}
}
