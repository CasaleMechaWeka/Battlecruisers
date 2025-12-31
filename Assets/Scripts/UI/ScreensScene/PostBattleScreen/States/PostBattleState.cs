using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public abstract class PostBattleState
    {
        protected readonly PostBattleScreenController _postBattleScreen;
        protected readonly MusicPlayer _musicPlayer;

        protected PostBattleState(
            PostBattleScreenController postBattleScreen,
            MusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, musicPlayer);

            _postBattleScreen = postBattleScreen;
            _musicPlayer = musicPlayer;
        }

        public virtual bool ShowDifficultySymbol => false;
        public virtual Difficulty Difficulty => DataProvider.SettingsManager.AIDifficulty;
        public virtual bool ShowVictoryBackground => true;
    }
}