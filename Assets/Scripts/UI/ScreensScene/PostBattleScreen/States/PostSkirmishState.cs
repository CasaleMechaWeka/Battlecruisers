using BattleCruisers.Data;
using BattleCruisers.Data.Skirmishes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    // FELIX  Handle both victory and defeat
    // FELIX  Avoid duplicate code with other states
    public class PostSkirmishState : IPostBattleState
    {
        private IPostBattleScreen _postBattleScreen;
        private ISkirmish _skirmish;
        private bool _userWonSkirmish;

        // FELIX  Remove async, convert to constructor
        public async Task InitialiseAsync(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            ISingleSoundPlayer soundPlayer,
            IMusicPlayer musicPlayer,
            IDifficultySpritesProvider difficultySpritesProvider)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, soundPlayer, musicPlayer, difficultySpritesProvider);
            Assert.IsNotNull(appModel.Skirmish);
            Assert.AreEqual(GameMode.Skirmish, appModel.Mode);

            _postBattleScreen = postBattleScreen;
            _skirmish = appModel.Skirmish;
            _userWonSkirmish = appModel.UserWonSkirmish;

            postBattleScreen.levelName.gameObject.SetActive(false);

            if (appModel.UserWonSkirmish)
            {
                postBattleScreen.title.text = VictoryState.VICTORY_TITLE_NO_LOOT;

                // FELIX  Avoid duplicate code with VictoryState?
                await postBattleScreen.completedDifficultySymbol.InitialiseAsync(_skirmish.Difficulty, difficultySpritesProvider);
                postBattleScreen.completedDifficultySymbol.gameObject.SetActive(true);
            }
            else
            {
                postBattleScreen.title.text = DefeatState.LOSS_TITLE;
            }
            // FELIX

            //postBattleScreen.title.color = Color.black;
            //postBattleScreen.levelName.levelName.color = Color.black;

            ////postBattleScreen.appraisalSection.Initialise(TUTORIAL_APPRAISAL_DRONE_TEXT, soundPlayer);
            //musicPlayer.PlayVictoryMusic();

            postBattleScreen.postSkirmishButtonsPanel.Initialise(postBattleScreen, soundPlayer, this, _userWonSkirmish);

            // Reset to default
            appModel.Mode = GameMode.Campaign;
            appModel.Skirmish = null;
            appModel.UserWonSkirmish = false;
        }

        public void RetrySkirmish()
        {
            _postBattleScreen.RetrySkirmish(_skirmish);
        }

        public bool ShowVictoryBackground()
        {
            return _userWonSkirmish;
        }
    }
}