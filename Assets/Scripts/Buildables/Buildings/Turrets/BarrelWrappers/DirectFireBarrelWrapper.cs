using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public class DirectFireBarrelWrapper : BarrelWrapper
    {
        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator(projectileStats);
		}
    }
}
