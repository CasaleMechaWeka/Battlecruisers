using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public abstract class LootItem<TItem> : ILootItem where TItem : IComparableItem
    {
        public IPrefabKey ItemKey { get; private set; }

        protected LootItem(IPrefabKey itemKey)
        {
            Assert.IsNotNull(itemKey);
            ItemKey = itemKey;
        }

        public void ShowItemDetails(IPrefabFactory prefabFactory, IItemDetailsGroup itemDetailsControllers)
        {
            TItem item = GetItem(prefabFactory);
            IComparableItemDetails<TItem> itemDetails = GetItemDetails(itemDetailsControllers);

            itemDetails.ShowItemDetails(item);
        }

        protected abstract TItem GetItem(IPrefabFactory prefabFactory);

        protected abstract IComparableItemDetails<TItem> GetItemDetails(IItemDetailsGroup itemDetailsControllers);

        public override bool Equals(object obj)
        {
            LootItem<TItem> other = obj as LootItem<TItem>;

            return
                other != null
                && ItemKey.SmartEquals(other.ItemKey);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(ItemKey);
        }
    }
}
