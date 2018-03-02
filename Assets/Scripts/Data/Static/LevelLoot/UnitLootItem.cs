using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class UnitLootItem : LootItem<IUnit>
    {
        protected override IUnit GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetUnitWrapperPrefab(ItemKey).Buildable;
        }

        protected override IComparableItemDetails<IUnit> GetItemDetails(IItemDetailsControllers itemDetailsControllers)
        {
            return itemDetailsControllers.UnitDetails;
        }
    }
}
