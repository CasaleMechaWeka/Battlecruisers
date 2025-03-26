using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public interface IItemDetailsManager
    {
        ItemFamily? SelectedItemFamily { get; }
        IBroadcastingProperty<int> NumOfDetailsShown { get; }
        IBroadcastingProperty<IComparableItem> SelectedItem { get; }
        IBroadcastingProperty<IComparableItem> ComparingItem { get; }
        HeckleDetailsController HeckleDetails { get; set; }

        void ShowDetails(IBuilding building);
        void ShowDetails(IBuilding building, ItemButton button);
        void ShowDetails(IUnit unit);
        void ShowDetails(IUnit unit, ItemButton button);
        void ShowDetails(ICruiser cruiser);
        void ShowDetails(HullType hullType);
        void ShowDetails(HeckleData heckleData);

        void CompareWithSelectedItem(IBuilding building);
        void CompareWithSelectedItem(IUnit unit);
        void CompareWithSelectedItem(ICruiser cruiser);

        void HideDetails();
    }
}