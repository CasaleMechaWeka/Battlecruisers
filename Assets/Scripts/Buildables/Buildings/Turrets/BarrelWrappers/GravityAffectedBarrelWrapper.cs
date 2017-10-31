using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class GravityAffectedBarrelWrapper : BarrelWrapper
	{
		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateArtilleryAngleCalculator();
		}
		
        // FELIX  Avoid duplicate code with MortarFireBarrelWrapper.  Parent class :)
        protected override IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, BarrelController barrel)
        {
            return 
                _factoryProvider.AccuracyAdjusterFactory.CreateGravityAffectedProjectileAdjuster(
                    angleCalculator, 
                    barrel.ProjectileStats.MaxVelocityInMPerS, 
                    barrel.TurretStats.Accuracy);
        }
	}
}
