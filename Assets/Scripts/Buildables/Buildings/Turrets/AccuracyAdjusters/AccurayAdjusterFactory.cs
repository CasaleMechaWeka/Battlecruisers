using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class AccuracyAdjusterFactory : IAccuracyAdjusterFactory
    {
        private const float TARGET_X_MARGIN_IN_M = 0.75f;
        private const float TARGET_Y_MARGIN_IN_M = 0.75f;

        public IAccuracyAdjuster CreateDummyAdjuster()
        {
            return new DummyAccuracyAdjuster();
        }

        public IAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(
            IAngleCalculator angleCalculator, 
            float projectileVelocityInMPerS, 
            ITurretStats turretStats)
        {
            return 
                CreateAccuracyAdjuster(
                    angleCalculator, 
                    projectileVelocityInMPerS, 
                    turretStats, 
                    new HorizontalTargetBoundsFinder(TARGET_X_MARGIN_IN_M));
        }

        public IAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(
            IAngleCalculator angleCalculator,
            float projectileVelocityInMPerS,
            ITurretStats turretStats)
        {
            return
                CreateAccuracyAdjuster(
                    angleCalculator,
                    projectileVelocityInMPerS,
                    turretStats,
                    new VerticalTargetBoundsFinder(TARGET_Y_MARGIN_IN_M));
        }

        private IAccuracyAdjuster CreateAccuracyAdjuster(
            IAngleCalculator angleCalculator, 
            float projectileVelocityInMPerS, 
            ITurretStats turretStats,
            ITargetBoundsFinder targetBoundsFinder)
        {
            return
                new AccuracyAdjuster(
                    targetBoundsFinder,
                    angleCalculator,
                    new LinearRangeFinder(),
                    new RandomGenerator(),
                    projectileVelocityInMPerS,
                    turretStats);
        }
    }
}
