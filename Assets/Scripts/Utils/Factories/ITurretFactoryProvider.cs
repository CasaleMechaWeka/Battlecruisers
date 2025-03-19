using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public interface ITurretFactoryProvider
    {
        IAngleCalculatorFactory AngleCalculatorFactory { get; }
        AngleLimiterFactory AngleLimiterFactory { get; }
        IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
    }
}