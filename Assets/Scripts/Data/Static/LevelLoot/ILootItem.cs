using BattleCruisers.Data.Models;
using BattleCruisers.UI.Common.BuildableDetails;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public interface ILootItem
    {
        /// <summary>
        /// Adds the item represented by this class to:
        /// 1. The game model's unlocked buildings/units/hulls
        /// 2. The player's loadout (if not a hull)
        /// </summary>
        void UnlockItem(GameModel gameModel);

        void ShowItemDetails(ItemDetailsGroupController itemDetailsControllers);
        bool IsUnlocked(GameModel gameModel);
    }
}
