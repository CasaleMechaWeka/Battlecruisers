using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public interface IItemDetailsPanel
    {
        IComparableItemDetails<ICruiser> LeftCruiserDetails { get; }
        IComparableItemDetails<ICruiser> RightCruiserDetails { get; }

        IComparableItemDetails<IBuilding> LeftBuildingDetails { get; }
        IComparableItemDetails<IBuilding> RightBuildingDetails { get; }

        IComparableItemDetails<IUnit> LeftUnitDetails { get; }
        IComparableItemDetails<IUnit> RightUnitDetails { get; }
    }
}