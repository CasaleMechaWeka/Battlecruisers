using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class MortarFireBarrelWrapper : BarrelWrapper
	{
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateMortarPredictor();
        }

		protected override IAngleCalculator CreateAngleCalculator()
		{
            return _factoryProvider.AngleCalculatorFactory.CreateMortarAngleCalculator();
		}

        protected override IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, BarrelController barrel)
        {
            return
                _factoryProvider.AccuracyAdjusterFactory.CreateVerticalImpactProjectileAdjuster(
                    angleCalculator,
                    barrel.ProjectileStats.MaxVelocityInMPerS,
                    barrel.TurretStats.Accuracy);
        }
	}
}
