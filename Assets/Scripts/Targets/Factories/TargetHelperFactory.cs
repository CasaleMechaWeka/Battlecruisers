using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Targets.Factories
{
    public class TargetHelperFactory : ITargetHelperFactory
    {
        public ITargetRangeHelper CreateShipRangeHelper(IShip ship)
		{
            return new ShipRangeHelper(ship);
		}
    }
}
