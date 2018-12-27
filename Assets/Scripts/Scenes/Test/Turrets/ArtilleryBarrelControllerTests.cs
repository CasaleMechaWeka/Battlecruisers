using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Scenes.Test
{
    public class ArtilleryBarrelControllerTests : BarrelControllerTestGod
	{
        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return new ArtilleryAngleCalculator(new AngleHelper(), projectileStats);
        }
    }
}
