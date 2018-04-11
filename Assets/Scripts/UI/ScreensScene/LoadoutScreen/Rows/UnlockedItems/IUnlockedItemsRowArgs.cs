using System.Collections.Generic;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public interface IUnlockedItemsRowArgs<TItem> where TItem : IComparableItem
    {
        IUIFactory UIFactory { get; }
        IList<TItem> UnlockedItems { get; }
        int NumOfLockedItems { get; }
        IItemsRow<TItem> ItemsRow { get; }
        IItemDetailsManager<TItem> DetailsManager { get; }
    }
}
