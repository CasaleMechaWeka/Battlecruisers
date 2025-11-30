using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Anti air turret
    /// Units:  Gunships
    /// </summary>
	public class LeadingDirectFireBarrelWrapper : BarrelWrapper
    {
        protected override ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return new LinearTargetPositionPredictor();
        }

        protected override IAngleCalculator CreateAngleCalculator(ProjectileStats projectileStats)
        {
            return new AngleCalculator();
        }
    }
}
