using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public interface IUnlockedItemsRowArgs<TItem> where TItem : class, IComparableItem
    {
        IUIFactory UIFactory { get; }
        IList<TItem> UnlockedItems { get; }
        int NumOfLockedItems { get; }
        IItemsRow<TItem> ItemsRow { get; }
    }
}
