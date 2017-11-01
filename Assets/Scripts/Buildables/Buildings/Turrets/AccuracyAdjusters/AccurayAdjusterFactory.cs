using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class AccuracyAdjusterFactory : IAccuracyAdjusterFactory
    {
        private const float TARGET_X_MARGIN_IN_M = 1;

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
                    new GravityAffectedTargetBoundsFinder(TARGET_X_MARGIN_IN_M),
                    angleCalculator,
                    new LinearRangeFinder(),
                    new RandomGenerator(),
                    projectileVelocityInMPerS,
                    accuracy);
        }
    }
}
