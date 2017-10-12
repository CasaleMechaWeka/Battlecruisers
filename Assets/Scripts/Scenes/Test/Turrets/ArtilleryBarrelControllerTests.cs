using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Scenes.Test
{
    public class ArtilleryBarrelControllerTests : BarrelControllerTestGod
	{
        protected override IAngleCalculator AngleCalculator { get { return new ArtilleryAngleCalculator(new TargetPositionPredictorFactory()); } }
	}
}
