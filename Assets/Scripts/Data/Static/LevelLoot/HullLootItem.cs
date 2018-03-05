using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class HullLootItem : LootItem<ICruiser, HullKey>
    {
        public HullLootItem(HullKey itemKey) : base(itemKey)
        {
        }

        protected override ICruiser GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetCruiserPrefab(_itemKey);
        }

        protected override IComparableItemDetails<ICruiser> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.HullDetails;
        }

        public override void UnlockItem(IGameModel gameModel)
        {
            gameModel.AddUnlockedHull(_itemKey);
        }
    }
}
