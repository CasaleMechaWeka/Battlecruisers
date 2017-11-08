using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Mortar, artillery, broadsides
    /// </summary>
    public class GravityAffectedBarrelWrapper : BarrelWrapper
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateArtilleryAngleCalculator();
		}
		
        protected override IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, BarrelController barrel)
        {
            return 
                _factoryProvider.AccuracyAdjusterFactory.CreateHorizontalImpactProjectileAdjuster(
                    angleCalculator, 
                    barrel.ProjectileStats.MaxVelocityInMPerS, 
                    barrel.TurretStats.Accuracy);
        }

        protected override AngleLimiters.IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateFacingLimiter();
        }
	}
}
