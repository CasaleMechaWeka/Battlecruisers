using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Scenes.Test
{
    public class TurretBarrelControllerTestGod : BarrelControllerTestGod 
	{
        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return new AngleCalculator(new AngleHelper());
        }
    }
}
