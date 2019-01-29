using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public interface IItemDetailsManager
    {
        ItemFamily? SelectedItemFamily { get; }
        IBroadcastingProperty<int> NumOfDetailsShown { get; }

        void ShowDetails(IBuilding building);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);

        void CompareWithSelectedItem(IBuilding building);
        void CompareWithSelectedItem(IUnit unit);
        void CompareWithSelectedItem(ICruiser cruiser);

        void HideDetails();
    }
}