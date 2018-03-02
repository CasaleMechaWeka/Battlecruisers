using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class HullLootItem : LootItem<ICruiser>
    {
        protected override ICruiser GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetCruiserPrefab(ItemKey);
        }

        protected override IComparableItemDetails<ICruiser> GetItemDetails(IItemDetailsControllers itemDetailsControllers)
        {
            return itemDetailsControllers.HullDetails;
        }
    }
}
