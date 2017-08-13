using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class DirectFireBarrelWrapper : ProximityTargetBarrelWrapper
    {
        protected override IAngleCalculator CreateAngleCalculator()
        {
            return _factoryProvider.AngleCalculatorFactory.CreateAngleCalcultor(_factoryProvider.TargetPositionPredictorFactory);
		}
    }
}
