using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Predictors;

namespace BattleCruisers.Scenes.Test
{
    public class TurretBarrelControllerTests : BarrelControllerTestGod 
	{
        protected override IAngleCalculator AngleCalculator { get { return new AngleCalculator(new TargetPositionPredictorFactory()); } }
	}
}
