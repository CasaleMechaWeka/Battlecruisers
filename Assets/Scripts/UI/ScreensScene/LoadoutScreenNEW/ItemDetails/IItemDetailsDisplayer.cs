using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public interface IItemDetailsDisplayer
    {
        ItemFamily? SelectedItemFamily { get; }

        void ShowDetails(IBuilding building);
        void ShowDetails(IUnit unit);
        void ShowDetails(ICruiser cruiser);

        void HideDetails();
    }
}