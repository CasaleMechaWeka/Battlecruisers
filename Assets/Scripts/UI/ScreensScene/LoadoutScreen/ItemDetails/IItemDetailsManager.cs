using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public interface IItemDetailsManager
    {
        ItemFamily? SelectedItemFamily { get; }
        IBroadcastingProperty<int> NumOfDetailsShown { get; }
        IBroadcastingProperty<IComparableItem> SelectedItem { get; }
        IBroadcastingProperty<IComparableItem> ComparingItem { get; }

        void ShowDetails(IBuilding building);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);

        void CompareWithSelectedItem(IBuilding building);
        void CompareWithSelectedItem(IUnit unit);
        void CompareWithSelectedItem(ICruiser cruiser);

        void HideDetails();
    }
}