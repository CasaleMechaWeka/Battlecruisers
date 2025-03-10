using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class AirFactoryVsAirFactoryTest : FactoryVsFactoryTest
    {
        protected override IAircraftProvider CreateAircraftProvider(Direction facingDirection)
        {
            return
                facingDirection == Direction.Right ?
                new AircraftProvider(parentCruiserPosition: _leftFactory.Position, enemyCruiserPosition: _rightFactory.Position, random: BCUtils.RandomGenerator.Instance) :
                new AircraftProvider(parentCruiserPosition: _rightFactory.Position, enemyCruiserPosition: _leftFactory.Position, random: BCUtils.RandomGenerator.Instance);
        }
    }
}
