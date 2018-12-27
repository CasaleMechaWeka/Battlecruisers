using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IAccuracyAdjusterFactory
    {
        IAccuracyAdjuster CreateDummyAdjuster();
        IAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats);
        IAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats);
    }
}
