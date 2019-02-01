using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
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

        public void ShowItemDetails(IPrefabFactory prefabFactory, IItemDetailsGroup itemDetailsControllers)
        {
            TItem item = GetItem(prefabFactory);
            IComparableItemDetails<TItem> itemDetails = GetItemDetails(itemDetailsControllers);

            itemDetails.ShowItemDetails(item);
        }

        protected abstract TItem GetItem(IPrefabFactory prefabFactory);

        protected abstract IComparableItemDetails<TItem> GetItemDetails(IItemDetailsGroup itemDetailsControllers);

        public abstract void UnlockItem(IGameModel gameModel);

        public override bool Equals(object obj)
        {
            LootItem<TItem, TKey> other = obj as LootItem<TItem, TKey>;

            return
                other != null
                && _itemKey.SmartEquals(other._itemKey);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(_itemKey);
        }
    }
}
