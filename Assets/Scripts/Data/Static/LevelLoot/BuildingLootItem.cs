using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class BuildingLootItem : LootItem<IBuilding>
    {
        public BuildingLootItem(IPrefabKey itemKey) : base(itemKey)
        {
        }

        protected override IBuilding GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetBuildingWrapperPrefab(ItemKey).Buildable;
        }

        protected override IComparableItemDetails<IBuilding> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.BuildingDetails;
        }
    }
}
