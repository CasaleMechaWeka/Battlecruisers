using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class PostSkirmishState : PostBattleState
    {
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

            _userWonSkirmish = appModel.UserWonSkirmish;

            postBattleScreen.levelName.gameObject.SetActive(false);

            if (appModel.UserWonSkirmish)
            {
                postBattleScreen.title.text = VictoryState.VICTORY_TITLE_NO_LOOT;
                postBattleScreen.title.color = Color.black;
                postBattleScreen.victoryNoLootMessage.SetActive(true);
                musicPlayer.PlayVictoryMusic();
            }
            else
            {
                postBattleScreen.title.text = DefeatState.LOSS_TITLE;
                postBattleScreen.defeatMessage.SetActive(true);
                musicPlayer.PlayDefeatMusic();
            }

            postBattleScreen.postSkirmishButtonsPanel.Initialise(postBattleScreen, soundPlayer, _userWonSkirmish);

            // Reset to default
            appModel.Mode = GameMode.Campaign;
            appModel.UserWonSkirmish = false;
        }

        public override bool ShowVictoryBackground => _userWonSkirmish;
        public override bool ShowDifficultySymbol => _userWonSkirmish;
        public override Difficulty Difficulty => _appModel.Skirmish.Difficulty;
    }
}