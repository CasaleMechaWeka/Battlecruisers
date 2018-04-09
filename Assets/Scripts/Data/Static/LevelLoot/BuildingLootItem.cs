using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class BuildingLootItem : LootItem<IBuilding, BuildingKey>
    {
        public BuildingLootItem(BuildingKey itemKey) : base(itemKey)
        {
        }

        protected override IBuilding GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetBuildingWrapperPrefab(_itemKey).Buildable;
        }

        protected override IComparableItemDetails<IBuilding> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.BuildingDetails;
        }

        public override void UnlockItem(IGameModel gameModel)
        {
            gameModel.AddUnlockedBuilding(_itemKey);
            gameModel.PlayerLoadout.AddBuilding(_itemKey);
        }
    }
}
