using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public interface ITurretFactoryProvider
    {
        IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        IAngleCalculatorFactory AngleCalculatorFactory { get; }
        IAngleLimiterFactory AngleLimiterFactory { get; }
        IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
    }
}