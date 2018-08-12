using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Mortar, artillery, broadsides
    /// </summary>
    public class GravityAffectedBarrelWrapper : BarrelWrapper
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateArtilleryAngleCalculator();
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
                        barrel.ProjectileStats.MaxVelocityInMPerS,
                        barrel.TurretStats);
            }
        }
	}
}
