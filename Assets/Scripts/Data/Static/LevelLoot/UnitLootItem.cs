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

        protected override IUnit GetItem()
        {
            return PrefabFactory.GetUnitWrapperPrefab(_itemKey).Buildable;
        }

        protected override IComparableItemDetails<IUnit> GetItemDetails(ItemDetailsGroupController itemDetailsControllers)
        {
            return itemDetailsControllers.UnitDetails;
        }

        public override void UnlockItem(GameModel gameModel)
        {
            if (!IsUnlocked(gameModel))
            {
                gameModel.AddUnlockedUnit(_itemKey);
                gameModel.PlayerLoadout.AddUnit(_itemKey);
            }
        }

        public override bool IsUnlocked(GameModel gameModel)
        {
            return gameModel.GetUnlockedUnits(_itemKey.UnitCategory).Contains(_itemKey);
        }
    }
}
