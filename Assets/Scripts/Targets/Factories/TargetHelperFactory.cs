using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Targets.Factories
{
    public static class TargetHelperFactory
    {
        public static ITargetRangeHelper CreateShipRangeHelper(IShip ship)
        {
            return new ShipRangeHelper(ship);
        }
    }
}
