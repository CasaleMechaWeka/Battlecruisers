using BattleCruisers.Data.Static.LevelLoot;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public interface ILootManager
    {
        bool ShouldShowLevelLoot(int levelNum);
        bool ShouldShowSideQuestLoot(int sideQuestID);

        /// <summary>
        /// Adds these items to the:
        ///     a) Game model unlocked items
        ///     b) User's loadout
        /// </summary>
        ILoot UnlockLevelLoot(int levelNum);

        void ShowLoot(ILoot unlockedLoot);
    }
}
