using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public abstract class LootItem<TItem, TKey> : ILootItem
        where TItem : IComparableItem
        where TKey : class, IPrefabKey
    {
        protected readonly TKey _itemKey;

        protected LootItem(TKey itemKey)
        {
            Assert.IsNotNull(itemKey);
            _itemKey = itemKey;
        }

        public void ShowItemDetails(ItemDetailsGroupController itemDetailsControllers)
        {
            TItem item = GetItem();
            IComparableItemDetails<TItem> itemDetails = GetItemDetails(itemDetailsControllers);

            itemDetails.ShowItemDetails(item);
        }

        protected abstract TItem GetItem();

        protected abstract IComparableItemDetails<TItem> GetItemDetails(ItemDetailsGroupController itemDetailsControllers);

        public abstract void UnlockItem(GameModel gameModel);

        public override bool Equals(object obj)
        {
            LootItem<TItem, TKey> other = obj as LootItem<TItem, TKey>;

            return
                other != null
                && _itemKey.SmartEquals(other._itemKey);
        }

        public abstract bool IsUnlocked(GameModel gameModel);

        public override int GetHashCode()
        {
            return this.GetHashCode(_itemKey);
        }
    }
}
