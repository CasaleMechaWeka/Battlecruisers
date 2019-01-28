using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public interface IItemDetailsDisplayer<TItem> where TItem : IComparableItem
    {
        void SelectItem(TItem item);
        void CompareWithSelectedItem(TItem item);
        void HideDetails();
    }
}