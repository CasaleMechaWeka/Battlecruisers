using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public abstract class PostBattleState : IPostBattleState
    {
        protected readonly PostBattleScreenController _postBattleScreen;
        protected readonly IApplicationModel _appModel;
        protected readonly IMusicPlayer _musicPlayer;
        protected readonly ILocTable _screensSceneStrings;

        protected PostBattleState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer,
            ILocTable screensSceneStrings)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, musicPlayer, screensSceneStrings);

            _postBattleScreen = postBattleScreen;
            _appModel = appModel;
            _musicPlayer = musicPlayer;
            _screensSceneStrings = screensSceneStrings;
        }

        public virtual bool ShowDifficultySymbol => false;
        public virtual Difficulty Difficulty => _appModel.DataProvider.SettingsManager.AIDifficulty;
        public virtual bool ShowVictoryBackground => true;
    }
}