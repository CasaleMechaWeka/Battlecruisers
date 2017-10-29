using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class DirectFireBarrelWrapper : BarrelWrapper
    {
        protected override IAngleCalculator CreateAngleCalculator()
        {
            return _factoryProvider.AngleCalculatorFactory.CreateAngleCalculator();
		}
    }
}
