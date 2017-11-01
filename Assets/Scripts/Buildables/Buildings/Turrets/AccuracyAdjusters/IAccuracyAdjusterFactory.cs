using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IAccuracyAdjusterFactory
    {
        IAccuracyAdjuster CreateDummyAdjuster();

        IAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(
            IAngleCalculator angleCalculator,
            float projectileVelocityInMPerS,
            float accuracy);

        IAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(
            IAngleCalculator angleCalculator,
            float projectileVelocityInMPerS,
            float accuracy);
    }
}
