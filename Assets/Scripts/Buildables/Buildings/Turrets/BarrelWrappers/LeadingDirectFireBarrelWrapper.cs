using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
	public class LeadingDirectFireBarrelWrapper : BarrelWrapper
	{
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor();
        }

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _factoryProvider.AngleCalculatorFactory.CreateLeadingAngleCalcultor(_factoryProvider.TargetPositionPredictorFactory);
		}
	}
}
