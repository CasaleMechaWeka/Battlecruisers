using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class StaticBarrelWrapper : BarrelWrapper
	{
        protected abstract float DesiredAngleInDegrees { get; }

		protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
		{
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateStaticAngleCalculator(DesiredAngleInDegrees);
		}

        protected override AngleLimiters.IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateDummyLimiter();
        }
	}
}
