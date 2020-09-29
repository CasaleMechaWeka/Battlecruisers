using BattleCruisers.Data.Static.LevelLoot;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public interface ILootManager
    {
        bool ShouldShowLoot(int levelNum);

        /// <summary>
        /// Adds these items to the:
        ///     a) Game model unlocked items
        ///     b) User's loadout
        /// </summary>
        ILoot UnlockLoot(int levelNum);

        void ShowLoot(ILoot unlockedLoot);
    }
}
