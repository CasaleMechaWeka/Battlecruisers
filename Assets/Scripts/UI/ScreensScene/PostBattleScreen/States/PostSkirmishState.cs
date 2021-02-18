using BattleCruisers.Data;
using BattleCruisers.Data.Skirmishes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    // FELIX  Handle both victory and defeat
    // FELIX  Avoid duplicate code with other states
    public class PostSkirmishState
    {
        private IPostBattleScreen _postBattleScreen;
        private ISkirmish _skirmish;

        public void Initialise(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            ISingleSoundPlayer soundPlayer,
            IMusicPlayer musicPlayer)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, soundPlayer, musicPlayer);

            _postBattleScreen = postBattleScreen;
            _skirmish = appModel.Skirmish;
            Assert.IsNotNull(_skirmish);

            postBattleScreen.levelName.gameObject.SetActive(false);

            // FELIX  Want to show difficulty

            // FELIX
            //postBattleScreen.title.text = TUTORIAL_TITLE;

            //postBattleScreen.title.color = Color.black;
            //postBattleScreen.levelName.levelName.color = Color.black;

            ////postBattleScreen.appraisalSection.Initialise(TUTORIAL_APPRAISAL_DRONE_TEXT, soundPlayer);
            //musicPlayer.PlayVictoryMusic();

            //postBattleScreen.postTutorialButtonsPanel.Initialise(postBattleScreen, soundPlayer, appModel.DataProvider.GameModel);

            // Reset to default
            appModel.Mode = GameMode.Campaign;
            appModel.Skirmish = null;
            appModel.UserWonSkirmish = false;
        }

        public void RetrySkirmish()
        {
            _postBattleScreen.RetrySkirmish(_skirmish);
        }
    }
}