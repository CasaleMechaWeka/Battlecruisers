using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IItemDetailsGroup
    {
        IComparableItemDetails<IBuilding> BuildingDetails { get; }
        IComparableItemDetails<IUnit> UnitDetails { get; }
        IComparableItemDetails<ICruiser> HullDetails { get; }
    }
}
