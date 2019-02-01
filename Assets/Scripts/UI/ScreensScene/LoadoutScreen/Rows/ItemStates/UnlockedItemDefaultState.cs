using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public class UnlockedItemDefaultState<TItem> : DefaultState<TItem> where TItem : class, IComparableItem
    {
        private readonly UnlockedItem<TItem> _unlockedItem;
        private readonly IItemsRow<TItem> _itemsRow;

        public UnlockedItemDefaultState(IItemsRow<TItem> itemsRow, UnlockedItem<TItem> item)
            : base(item)
        {
            Assert.IsNotNull(itemsRow);

            _itemsRow = itemsRow;
            _unlockedItem = item;
        }

        public override void SelectItem()
        {
            _unlockedItem.IsItemInLoadout = _itemsRow.SelectUnlockedItem(_unlockedItem);
        }
    }
}
