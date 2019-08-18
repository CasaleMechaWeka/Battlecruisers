using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class AirFactoryVsAirFactoryTest : FactoryVsFactoryTest
    {
        protected override IAircraftProvider CreateAircraftProvider(Direction facingDirection)
        {
            BCUtils.IRandomGenerator random = new BCUtils.RandomGenerator();

            return
                facingDirection == Direction.Right ?
                new AircraftProvider(parentCruiserPosition: _leftFactory.Position, enemyCruiserPosition: _rightFactory.Position, random: random) :
                new AircraftProvider(parentCruiserPosition: _rightFactory.Position, enemyCruiserPosition: _leftFactory.Position, random: random);
        }
    }
}
