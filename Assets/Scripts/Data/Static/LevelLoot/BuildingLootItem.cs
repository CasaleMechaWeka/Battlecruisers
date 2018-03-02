using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class BuildingLootItem : LootItem<IBuilding>
    {
        protected override IBuilding GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetBuildingWrapperPrefab(ItemKey).Buildable;
        }

        protected override IComparableItemDetails<IBuilding> GetItemDetails(IItemDetailsControllers itemDetailsControllers)
        {
            return itemDetailsControllers.BuildingDetails;
        }
    }
}
