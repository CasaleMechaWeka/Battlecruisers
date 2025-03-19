using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public class TurretFactoryProvider : ITurretFactoryProvider
    {
        public IAngleCalculatorFactory AngleCalculatorFactory { get; }
        public AngleLimiterFactory AngleLimiterFactory { get; }
        public IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }

        public TurretFactoryProvider()
        {
            AngleCalculatorFactory = new AngleCalculatorFactory();
            AngleLimiterFactory = new AngleLimiterFactory();
            AttackablePositionFinderFactory = new AttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
        }
    }
}