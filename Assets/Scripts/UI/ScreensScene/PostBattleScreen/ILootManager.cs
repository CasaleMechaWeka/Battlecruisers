namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public interface ILootManager
    {
        /// <summary>
        /// Shoulds the show loot.
        /// </summary>
        /// <returns><c>true</c>, if show loot was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="levelNum">Level number.</param>
        bool ShouldShowLoot(int levelNum);

        /// <summary>
        /// 1. Increments the number of completed levels in the game model
        /// 2. Adds these items to the:
        ///     a) Game model unlocked items
        ///     b) User's loadout
		/// 3. Displays the loot items for the given level
        /// </summary>
        void UnlockLoot(int levelNum);
    }
}
