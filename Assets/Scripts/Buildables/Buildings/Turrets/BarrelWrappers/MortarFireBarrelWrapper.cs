using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MortarFireBarrelWrapper : BarrelWrapper
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateMortarAngleCalcultor(_factoryProvider.TargetPositionPredictorFactory);
		}
	}
}
