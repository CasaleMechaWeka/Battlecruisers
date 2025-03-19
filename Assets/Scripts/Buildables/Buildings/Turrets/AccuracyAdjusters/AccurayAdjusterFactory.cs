using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class AccuracyAdjusterFactory
    {
        private const float TARGET_X_MARGIN_IN_M = 0.75f;
        private const float TARGET_Y_MARGIN_IN_M = 0.75f;

        public IAccuracyAdjuster CreateDummyAdjuster()
        {
            return new DummyAccuracyAdjuster();
        }

        public IAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats)
        {
            return
                CreateAccuracyAdjuster(
                    angleCalculator,
                    turretStats,
                    (TARGET_X_MARGIN_IN_M, 0f));
        }

        public IAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats)
        {
            return
                CreateAccuracyAdjuster(
                    angleCalculator,
                    turretStats,
                    (0f, TARGET_Y_MARGIN_IN_M));
        }

        private IAccuracyAdjuster CreateAccuracyAdjuster(
            IAngleCalculator angleCalculator,
            ITurretStats turretStats,
            (float x, float y) targetMargins)
        {
            return
                new AccuracyAdjuster(
                    targetMargins,
                    angleCalculator,
                    RandomGenerator.Instance,
                    turretStats);
        }
    }
}
