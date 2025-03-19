using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Utils.Factories
{
    public class TurretFactoryProvider
    {
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }

        public TurretFactoryProvider()
        {
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
        }
    }
}