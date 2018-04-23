using System.Collections.Generic;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedItemsRowArgs<TItem> : IUnlockedItemsRowArgs<TItem> where TItem : IComparableItem
    {
        public IUIFactory UIFactory { get; private set; }
        public IList<TItem> UnlockedItems { get; private set; }
		public int NumOfLockedItems { get; private set; }
        public IItemsRow<TItem> ItemsRow { get; private set; }

        public UnlockedItemsRowArgs(
            IUIFactory uiFactory, 
            IList<TItem> unlockedItems, 
            int numOfLockedItems,
            IItemsRow<TItem> itemsRow)
        {
            Helper.AssertIsNotNull(uiFactory, unlockedItems, itemsRow);

            UIFactory = uiFactory;
            UnlockedItems = unlockedItems;
            NumOfLockedItems = numOfLockedItems;
            ItemsRow = itemsRow;
        }
    }
}
