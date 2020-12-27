using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class VictoryState
    {
        private PostBattleScreenController _postBattleScreen;
        private ILootManager _lootManager;
        private ILoot _unlockedLoot;

        private const string VICTORY_TITLE_NO_LOOT = "Sweet as!";
        private const string VICTORY_TITLE_LOOT = "Found some Schematics!";
        private const int VICTORY_TITLE_LOOT_FONT_SIZE = 125;

        public async Task InitialiseAsync(
            PostBattleScreenController postBattleScreen,
            ISingleSoundPlayer soundPlayer,
            IMusicPlayer musicPlayer,
            IDataProvider dataProvider,
            IDifficultySpritesProvider difficultySpritesProvider,
            ILootManager lootManager,
            ITrashTalkData levelTrashTalkData,
            PostBattleScreenBehaviour desiredBehaviour)
        {
            Helper.AssertIsNotNull(postBattleScreen, soundPlayer, musicPlayer, dataProvider, difficultySpritesProvider, lootManager, levelTrashTalkData);

            _postBattleScreen = postBattleScreen;
            _lootManager = lootManager;

            BattleResult battleResult = dataProvider.GameModel.LastBattleResult;

            postBattleScreen.title.text = VICTORY_TITLE_NO_LOOT;
            postBattleScreen.title.color = Color.black;
            postBattleScreen.levelName.levelName.color = Color.black;
            musicPlayer.PlayVictoryMusic();
            await postBattleScreen.completedDifficultySymbol.InitialiseAsync(dataProvider.SettingsManager.AIDifficulty, difficultySpritesProvider);
            postBattleScreen.completedDifficultySymbol.gameObject.SetActive(true);

            if (desiredBehaviour == PostBattleScreenBehaviour.Victory_DemoCompleted
                || (desiredBehaviour == PostBattleScreenBehaviour.Default
                    && dataProvider.StaticData.IsDemo
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
                    postBattleScreen.appraisalSection.Initialise(levelTrashTalkData.AppraisalDroneText, soundPlayer);
                }
            }

            CompletedLevel level = new CompletedLevel(levelNum: battleResult.LevelNum, hardestDifficulty: dataProvider.SettingsManager.AIDifficulty);
            dataProvider.GameModel.AddCompletedLevel(level);
            dataProvider.SaveGame();
        }

        public void ShowLoot()
        {
            Assert.IsNotNull(_unlockedLoot);

            _postBattleScreen.appraisalSection.gameObject.SetActive(false);
            _postBattleScreen.postBattleButtonsPanel.gameObject.SetActive(true);
            _postBattleScreen.unlockedItemSection.Show();
            _lootManager.ShowLoot(_unlockedLoot);
        }
    }
}