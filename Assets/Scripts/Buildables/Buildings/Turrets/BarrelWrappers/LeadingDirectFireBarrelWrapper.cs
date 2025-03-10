using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Anti air turret
    /// Units:  Gunships
    /// </summary>
	public abstract class LeadingDirectFireBarrelWrapper : BarrelWrapper
	{
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor();
        }

		protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
		{
			return _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator();
		}
	}
}
