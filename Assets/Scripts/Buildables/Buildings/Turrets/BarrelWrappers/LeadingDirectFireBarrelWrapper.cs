using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Anti air turret
    /// </summary>
	public class LeadingDirectFireBarrelWrapper : BarrelWrapper
	{
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor();
        }

		protected override IAngleCalculator CreateAngleCalculator()
		{
			return _factoryProvider.AngleCalculatorFactory.CreateAngleCalculator();
		}

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateAntiAirLimiter();
        }
	}
}
