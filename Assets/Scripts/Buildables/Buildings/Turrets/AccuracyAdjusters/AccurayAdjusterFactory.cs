using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class AccuracyAdjusterFactory : IAccuracyAdjusterFactory
    {
        public IAccuracyAdjuster CreateDummyAdjuster()
        {
            return new DummyAccuracyAdjuster();
        }

        public IAccuracyAdjuster CreateGravityAffectedProjectileAdjuster(
            IAngleCalculator angleCalculator, 
            float projectileVelocityInMPerS, 
            float accuracy)
        {
            return
                new AccuracyAdjuster(
                    new GravityAffectedTargetBoundsFinder(),
                    angleCalculator,
                    new LinearRangeFinder(),
                    new RandomGenerator(),
                    projectileVelocityInMPerS,
                    accuracy);
        }
    }
}
