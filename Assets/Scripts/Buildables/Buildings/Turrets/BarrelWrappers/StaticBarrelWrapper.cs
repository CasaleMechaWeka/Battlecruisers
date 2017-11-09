using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class StaticBarrelWrapper : BarrelWrapper
	{
        protected abstract float DesiredAngleInDegrees { get; }

		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateStaticAngleCalculator(DesiredAngleInDegrees);
		}

        protected override AngleLimiters.IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateDummyLimiter();
        }
	}
}
