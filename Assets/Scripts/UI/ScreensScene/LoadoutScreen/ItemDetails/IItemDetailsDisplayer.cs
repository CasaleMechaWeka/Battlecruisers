using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public interface IItemDetailsDisplayer<TItem> where TItem : IComparableItem
    {
        IBroadcastingProperty<TItem> SelectedItem { get; }

        void SelectItem(TItem item);
        void CompareWithSelectedItem(TItem item);
        void HideDetails();
    }
}