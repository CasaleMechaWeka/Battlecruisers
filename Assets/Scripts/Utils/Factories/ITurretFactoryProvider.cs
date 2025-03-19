using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public interface ITurretFactoryProvider
    {
        IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
    }
}