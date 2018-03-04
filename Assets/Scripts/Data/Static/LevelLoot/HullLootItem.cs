using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class HullLootItem : LootItem<ICruiser>
    {
        public HullLootItem(IPrefabKey itemKey) : base(itemKey)
        {
        }

        protected override ICruiser GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetCruiserPrefab(ItemKey);
        }

        protected override IComparableItemDetails<ICruiser> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.HullDetails;
        }
    }
}
