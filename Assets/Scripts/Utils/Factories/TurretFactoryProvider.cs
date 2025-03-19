using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public class TurretFactoryProvider
    {
        public AttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }

        public TurretFactoryProvider()
        {
            AttackablePositionFinderFactory = new AttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
        }
    }
}