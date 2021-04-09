using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class VictoryState : PostBattleState
    {
        private ILootManager _lootManager;
        private ILoot _unlockedLoot;

        public const string VICTORY_TITLE_NO_LOOT = "Sweet as!";
        private const string VICTORY_TITLE_LOOT = "Found some Schematics!";
        private const int VICTORY_TITLE_LOOT_FONT_SIZE = 125;

        public VictoryState(
            PostBattleScreenController postBattleScreen, 
            IApplicationModel appModel, 
            IMusicPlayer musicPlayer,
            ISingleSoundPlayer soundPlayer,
            ILootManager lootManager,
            ITrashTalkData levelTrashTalkData,
            PostBattleScreenBehaviour desiredBehaviour)
            : base(postBattleScreen, appModel, musicPlayer)
        {
            Helper.AssertIsNotNull(soundPlayer, lootManager, levelTrashTalkData);

            _lootManager = lootManager;

            BattleResult battleResult = _appModel.DataProvider.GameModel.LastBattleResult;

            postBattleScreen.title.text = VICTORY_TITLE_NO_LOOT;
            postBattleScreen.title.color = Color.black;
            postBattleScreen.levelName.levelName.color = Color.black;
            musicPlayer.PlayVictoryMusic();

            if (desiredBehaviour == PostBattleScreenBehaviour.Victory_DemoCompleted
                || (desiredBehaviour == PostBattleScreenBehaviour.Default
                    && _appModel.DataProvider.StaticData.IsDemo
                    && battleResult.LevelNum == StaticData.NUM_OF_LEVELS_IN_DEMO))
            {
                postBattleScreen.demoCompletedScreen.SetActive(true);
                postBattleScreen.demoHomeButton.Initialise(soundPlayer, postBattleScreen.GoToHomeScreen);
                lootManager.UnlockLoot(battleResult.LevelNum);
            }
            else
            {
                if (desiredBehaviour == PostBattleScreenBehaviour.Victory_LootUnlocked
                    || (desiredBehaviour == PostBattleScreenBehaviour.Default
                    && _lootManager.ShouldShowLoot(battleResult.LevelNum)))
                {
                    postBattleScreen.title.text = VICTORY_TITLE_LOOT;
                    postBattleScreen.title.fontSize = VICTORY_TITLE_LOOT_FONT_SIZE;

                    _postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(false);
                    postBattleScreen.appraisalSection.Initialise(levelTrashTalkData.AppraisalDroneText, soundPlayer, ShowLoot);
                    _unlockedLoot = lootManager.UnlockLoot(battleResult.LevelNum);
                }
                else
                {
                    _postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
                    postBattleScreen.appraisalSection.Initialise(levelTrashTalkData.AppraisalDroneText, soundPlayer);
                }
            }

            CompletedLevel level = new CompletedLevel(levelNum: battleResult.LevelNum, hardestDifficulty: _appModel.DataProvider.SettingsManager.AIDifficulty);
            _appModel.DataProvider.GameModel.AddCompletedLevel(level);
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