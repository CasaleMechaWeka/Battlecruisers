using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    public interface IItemDetailsControllers
    {
        IComparableItemDetails<IBuilding> BuildingDetails { get; }
        IComparableItemDetails<IUnit> UnitDetails { get; }
        IComparableItemDetails<ICruiser> HullDetails { get; }
    }
}
