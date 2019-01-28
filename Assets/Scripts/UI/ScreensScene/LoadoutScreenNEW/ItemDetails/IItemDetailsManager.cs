using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public interface IItemDetailsManager
    {
        ItemFamily? SelectedItemFamily { get; }
        int NumOfDetailsShown { get; }

        event EventHandler NumOfDetailsShownChanged;

        void ShowDetails(IBuilding building);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);

        void CompareWithSelectedItem(IBuilding building);
        void CompareWithSelectedItem(IUnit unit);
        void CompareWithSelectedItem(ICruiser cruiser);

        void HideDetails();
    }
}