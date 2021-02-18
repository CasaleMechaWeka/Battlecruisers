using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public abstract class PostBattleState : IPostBattleState
    {
        protected readonly PostBattleScreenController _postBattleScreen;
        protected readonly IApplicationModel _appModel;
        protected readonly IMusicPlayer _musicPlayer;

        protected PostBattleState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, musicPlayer);

            _postBattleScreen = postBattleScreen;
            _appModel = appModel;
            _musicPlayer = musicPlayer;
        }

        public virtual bool ShowDifficultySymbol => false;
        public virtual Difficulty Difficulty => _appModel.DataProvider.SettingsManager.AIDifficulty;
        public virtual bool ShowVictoryBackground => true;
    }
}