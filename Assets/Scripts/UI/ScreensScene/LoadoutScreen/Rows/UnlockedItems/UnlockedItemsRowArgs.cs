using System.Collections.Generic;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedItemsRowArgs<TItem> : IUnlockedItemsRowArgs<TItem> where TItem : IComparableItem
    {
        public IUIFactory UIFactory { get; private set; }
        public IList<TItem> UnlockedItems { get; private set; }
        public IItemsRow<TItem> ItemsRow { get; private set; }
        public IItemDetailsManager<TItem> DetailsManager { get; private set; }

        public UnlockedItemsRowArgs(IUIFactory uiFactory, IList<TItem> unlockedItems, IItemsRow<TItem> itemsRow, IItemDetailsManager<TItem> detailsManager)
        {
            Helper.AssertIsNotNull(uiFactory, unlockedItems, itemsRow, detailsManager);

            UIFactory = uiFactory;
            UnlockedItems = unlockedItems;
            ItemsRow = itemsRow;
            DetailsManager = detailsManager;            
        }
    }
}
