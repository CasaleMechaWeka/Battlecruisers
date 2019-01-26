using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public interface IItemDetailsDisplayer
    {
        TargetType SelectedItemType { get; }

        // FELIX  Remove event?
        event EventHandler SelectedItemTypeChanged;

        void ShowDetails(IBuilding building);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);
    }
}