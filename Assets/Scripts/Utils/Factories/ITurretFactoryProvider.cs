using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public interface ITurretFactoryProvider
    {
        AngleCalculatorFactory AngleCalculatorFactory { get; }
        IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
    }
}