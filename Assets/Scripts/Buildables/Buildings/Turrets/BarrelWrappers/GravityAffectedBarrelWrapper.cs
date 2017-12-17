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
            if (barrel.TurretStats.Accuracy >= AccuracyAdjuster.MAX_ACCURACY)
            {
                return _factoryProvider.AccuracyAdjusterFactory.CreateDummyAdjuster();
            }
            else
            {
                return
                    _factoryProvider.AccuracyAdjusterFactory.CreateHorizontalImpactProjectileAdjuster(
                        angleCalculator,
                        barrel.ProjectileStats.MaxVelocityInMPerS,
                        barrel.TurretStats.Accuracy);
            }
        }
	}
}
