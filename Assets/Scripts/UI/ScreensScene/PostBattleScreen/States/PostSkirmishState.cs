using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Skirmishes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    // FELIX  Handle both victory and defeat
    // FELIX  Avoid duplicate code with other states
    public class PostSkirmishState : PostBattleState
    {
        private ISkirmish _skirmish;
        private bool _userWonSkirmish;

        public PostSkirmishState(
            PostBattleScreenController postBattleScreen, 
            IApplicationModel appModel, 
            IMusicPlayer musicPlayer,
            ISingleSoundPlayer soundPlayer)
            : base(postBattleScreen, appModel, musicPlayer)
        {
            Assert.IsNotNull(soundPlayer);
            Assert.IsNotNull(appModel.Skirmish);
            Assert.AreEqual(GameMode.Skirmish, appModel.Mode);

            _skirmish = appModel.Skirmish;
            _userWonSkirmish = appModel.UserWonSkirmish;

            postBattleScreen.levelName.gameObject.SetActive(false);

            if (appModel.UserWonSkirmish)
            {
                postBattleScreen.title.text = VictoryState.VICTORY_TITLE_NO_LOOT;
            }
            else
            {
                postBattleScreen.title.text = DefeatState.LOSS_TITLE;
            }
            // FELIX  Choose colour based on victory/defeat
            //postBattleScreen.title.color = Color.black;
            //postBattleScreen.levelName.levelName.color = Color.black;

            // FELIX  Choose text based on victory/defeat
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

        public override bool ShowVictoryBackground => _userWonSkirmish;
        public override bool ShowDifficultySymbol => _userWonSkirmish;
        public override Difficulty Difficulty => _skirmish.Difficulty;
    }
}