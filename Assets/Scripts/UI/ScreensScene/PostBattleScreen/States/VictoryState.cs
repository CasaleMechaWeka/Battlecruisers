using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class VictoryState : PostBattleState
    {
        private ILootManager _lootManager;
        private ILoot _unlockedLoot;
        public const string VICTORY_TITLE_NO_LOOT_KEY = "UI/PostBattleScreen/Title/VictoryNoLoot";
        private const string VICTORY_TITLE_LOOT_KEY = "UI/PostBattleScreen/Title/VictoryLoot";

        public VictoryState(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            IMusicPlayer musicPlayer,
            ISingleSoundPlayer soundPlayer,
            ILootManager lootManager,
            ITrashTalkData trashTalkData,
            PostBattleScreenBehaviour desiredBehaviour)
            : base(postBattleScreen, appModel, musicPlayer)
        {
            Helper.AssertIsNotNull(soundPlayer, lootManager, trashTalkData);


            _lootManager = lootManager;

            BattleResult battleResult = _appModel.DataProvider.GameModel.LastBattleResult;

            postBattleScreen.title.text = LocTableFactory.ScreensSceneTable.GetString(VICTORY_TITLE_NO_LOOT_KEY);
            postBattleScreen.title.color = Color.black;
            postBattleScreen.levelName.levelName.color = Color.black;
            //musicPlayer.PlayVictoryMusic(); 

#if IS_DEMO
    // (demo branch unchanged)
#else
            if (appModel.Mode != GameMode.SideQuest &&
               (desiredBehaviour == PostBattleScreenBehaviour.Victory_LootUnlocked ||
                (desiredBehaviour == PostBattleScreenBehaviour.Default &&
                 _lootManager.ShouldShowLevelLoot(battleResult.LevelNum))))
            {
                postBattleScreen.title.text = LocTableFactory.ScreensSceneTable.GetString(VICTORY_TITLE_LOOT_KEY);

                _postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(false);
                postBattleScreen.appraisalSection.Initialise(trashTalkData.AppraisalDroneText, soundPlayer, ShowLoot);
                _unlockedLoot = lootManager.UnlockLevelLoot(battleResult.LevelNum);
            }
            else if (appModel.Mode == GameMode.SideQuest &&
                    (desiredBehaviour == PostBattleScreenBehaviour.Victory_SideQuest_LootUnlocked ||
                     (desiredBehaviour == PostBattleScreenBehaviour.Default &&
                      _lootManager.ShouldShowSideQuestLoot(_appModel.SelectedSideQuestID))))
            {
                postBattleScreen.title.text = LocTableFactory.ScreensSceneTable.GetString(VICTORY_TITLE_LOOT_KEY);

                _postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(false);
                postBattleScreen.appraisalSection.Initialise(trashTalkData.AppraisalDroneText, soundPlayer, ShowLoot);
                _unlockedLoot = lootManager.UnlockSideQuestLoot(_appModel.SelectedSideQuestID);
            }
            else
            {
                if (appModel.Mode == GameMode.CoinBattle)
                {
                    postBattleScreen.victoryNoLootMessage.gameObject.SetActive(true);
                    postBattleScreen.postSkirmishButtonsPanel.gameObject.SetActive(true);
                    musicPlayer.PlayVictoryMusic();
                }
                else
                {
                    postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
                    postBattleScreen.appraisalSection.Initialise(trashTalkData.AppraisalDroneText, soundPlayer);
                }
            }
#endif

            if (appModel.Mode == GameMode.CoinBattle)
            {
                //Reset gamemode to Campaign
                appModel.Mode = GameMode.Campaign;
            }
            else
            {
                SaveVictory(battleResult);
            }
        }

        private void SaveVictory(BattleResult battleResult)
        {
            CompletedLevel level = new CompletedLevel(levelNum: battleResult.LevelNum, hardestDifficulty: _appModel.DataProvider.SettingsManager.AIDifficulty);
            if (_appModel.Mode != GameMode.SideQuest)
                _appModel.DataProvider.GameModel.AddCompletedLevel(level);
            else
                _appModel.DataProvider.GameModel.AddCompletedSideQuest(level);

            int nextLevel = 0;
            if (_appModel.Mode != GameMode.SideQuest)
                nextLevel = _appModel.SelectedLevel + 1;
            if (nextLevel <= _appModel.DataProvider.LockedInfo.NumOfLevelsUnlocked)
                _appModel.DataProvider.GameModel.SelectedLevel = nextLevel;

            _appModel.DataProvider.SaveGame();
        }

        public void ShowLoot()
        {
            Assert.IsNotNull(_unlockedLoot);

            _postBattleScreen.appraisalSection.gameObject.SetActive(false);
            _postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
            _postBattleScreen.unlockedItemSection.Show();
            _lootManager.ShowLoot(_unlockedLoot);
        }

        public override bool ShowDifficultySymbol => true;
    }
}