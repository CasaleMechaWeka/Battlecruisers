using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Targets.Helpers;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetHelperFactory
    {
        ITargetRangeHelper CreateShipRangeHelper(IShip ship);
    }
}