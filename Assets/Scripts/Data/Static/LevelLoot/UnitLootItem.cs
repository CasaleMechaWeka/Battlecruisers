using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class UnitLootItem : LootItem<IUnit>
    {
        public UnitLootItem(IPrefabKey itemKey) : base(itemKey)
        {
        }

        protected override IUnit GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetUnitWrapperPrefab(ItemKey).Buildable;
        }

        protected override IComparableItemDetails<IUnit> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.UnitDetails;
        }
    }
}
