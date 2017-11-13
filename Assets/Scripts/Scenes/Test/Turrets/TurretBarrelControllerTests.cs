using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Scenes.Test
{
    public class TurretBarrelControllerTests : BarrelControllerTestGod 
	{
        protected override IAngleCalculator AngleCalculator { get { return new AngleCalculator(new AngleHelper()); } }
	}
}
