using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class HullLootItem : LootItem<ICruiser, HullKey>
    {
        public HullLootItem(HullKey itemKey) : base(itemKey)
        {
        }

        protected override ICruiser GetItem(PrefabFactory prefabFactory)
        {
            return prefabFactory.GetCruiserPrefab(_itemKey);
        }

        protected override IComparableItemDetails<ICruiser> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.HullDetails;
        }

        public override void UnlockItem(IGameModel gameModel)
        {
            if (!IsUnlocked(gameModel))
                gameModel.AddUnlockedHull(_itemKey);
        }

        public override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedHulls.Contains(_itemKey);
        }
    }
}
