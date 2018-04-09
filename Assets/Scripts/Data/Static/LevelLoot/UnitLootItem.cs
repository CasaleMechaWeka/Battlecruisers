using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class UnitLootItem : LootItem<IUnit, UnitKey>
    {
        public UnitLootItem(UnitKey itemKey) : base(itemKey)
        {
        }

        protected override IUnit GetItem(IPrefabFactory prefabFactory)
        {
            return prefabFactory.GetUnitWrapperPrefab(_itemKey).Buildable;
        }

        protected override IComparableItemDetails<IUnit> GetItemDetails(IItemDetailsGroup itemDetailsControllers)
        {
            return itemDetailsControllers.UnitDetails;
        }

        public override void UnlockItem(IGameModel gameModel)
        {
            gameModel.AddUnlockedUnit(_itemKey);
            gameModel.PlayerLoadout.AddUnit(_itemKey);
        }
    }
}
