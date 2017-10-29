using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Scenes.Test
{
    public class ArtilleryBarrelControllerTests : BarrelControllerTestGod
	{
        protected override IAngleCalculator AngleCalculator { get { return new ArtilleryAngleCalculator(); } }
	}
}
