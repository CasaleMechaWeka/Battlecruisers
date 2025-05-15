using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class PostSkirmishState : PostBattleState
    {
        private bool _userWonSkirmish;

        public PostSkirmishState(
            PostBattleScreenController postBattleScreen,
            IMusicPlayer musicPlayer,
            SingleSoundPlayer soundPlayer)
            : base(postBattleScreen, musicPlayer)
        {
            Assert.IsNotNull(soundPlayer);
            Assert.IsNotNull(DataProvider.GameModel.Skirmish);
            //Assert.AreEqual(GameMode.Skirmish, ApplicationModel.Mode);

            _userWonSkirmish = ApplicationModel.UserWonSkirmish;

            postBattleScreen.levelName.gameObject.SetActive(false);

            if (ApplicationModel.UserWonSkirmish)
            {
                postBattleScreen.title.text = LocTableCache.ScreensSceneTable.GetString(VictoryState.VICTORY_TITLE_NO_LOOT_KEY);
                postBattleScreen.title.color = Color.black;
                postBattleScreen.victoryNoLootMessage.SetActive(true);
                musicPlayer.PlayVictoryMusic();
            }
            else
            {
                postBattleScreen.title.text = LocTableCache.ScreensSceneTable.GetString(DefeatState.LOSS_TITLE_KEY);
                postBattleScreen.defeatMessage.SetActive(true);
                musicPlayer.PlayDefeatMusic();
            }

            postBattleScreen.postSkirmishButtonsPanel.Initialise(postBattleScreen, soundPlayer, _userWonSkirmish);

            // Reset to default
            ApplicationModel.Mode = GameMode.Campaign;
            ApplicationModel.UserWonSkirmish = false;
        }

        public override bool ShowVictoryBackground => _userWonSkirmish;
        public override bool ShowDifficultySymbol => _userWonSkirmish;
        public override Difficulty Difficulty => DataProvider.GameModel.Skirmish.Difficulty;
    }
}