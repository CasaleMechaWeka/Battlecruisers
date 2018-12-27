using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Mortar, artillery, broadsides
    /// </summary>
    public class GravityAffectedBarrelWrapper : BarrelWrapper
	{
		protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
		{
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateArtilleryAngleCalculator(projectileStats);
		}
		
        protected override IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, IBarrelController barrel)
        {
            if (barrel.TurretStats.Accuracy >= Constants.MAX_ACCURACY)
            {
                return _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster();
            }
            else
            {
                return
                    _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateHorizontalImpactProjectileAdjuster(
                        angleCalculator,
                        barrel.TurretStats);
            }
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateGravityAffectedLimiter();
        }
    }
}
