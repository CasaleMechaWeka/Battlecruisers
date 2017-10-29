using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IAccuracyAdjusterFactory
    {
        IAccuracyAdjuster CreateDummyAdjuster();

        IAccuracyAdjuster CreateGravityAffectedProjectileAdjuster(
            IAngleCalculator angleCalculator,
            float projectileVelocityInMPerS,
            float accuracy);
    }
}
