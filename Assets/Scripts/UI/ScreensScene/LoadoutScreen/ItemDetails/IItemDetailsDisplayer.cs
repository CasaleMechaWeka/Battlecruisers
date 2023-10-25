using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public interface IItemDetailsDisplayer<TItem> where TItem : IComparableItem
    {
        IBroadcastingProperty<TItem> SelectedItem { get; }

        void SelectItem(TItem item);
        void SelectItem(HullType hullType);
        void CompareWithSelectedItem(TItem item);
        void HideDetails();
    }
}