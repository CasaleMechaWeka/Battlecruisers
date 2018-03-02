using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public abstract class LootItem<TItem> : ILootItem where TItem : IComparableItem
    {
        public IPrefabKey ItemKey { get; private set; }

        public void ShowItemDetails(IPrefabFactory prefabFactory, IItemDetailsControllers itemDetailsControllers)
        {
            TItem item = GetItem(prefabFactory);
            IComparableItemDetails<TItem> itemDetails = GetItemDetails(itemDetailsControllers);

            itemDetails.ShowItemDetails(item);
        }

        protected abstract TItem GetItem(IPrefabFactory prefabFactory);

        protected abstract IComparableItemDetails<TItem> GetItemDetails(IItemDetailsControllers itemDetailsControllers);
    }
}
