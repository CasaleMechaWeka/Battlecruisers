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
    // FELIX  Test all states :)
    public class VictoryState
    {
        private PostBattleScreenController _postBattleScreen;
        private ILootManager _lootManager;
        private ILoot _unlockedLoot;

        private const string VICTORY_TITLE = "Sweet as!";

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

            postBattleScreen.title.text = VICTORY_TITLE;
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
            }
            else if (desiredBehaviour == PostBattleScreenBehaviour.Victory_GameCompleted
                || (desiredBehaviour == PostBattleScreenBehaviour.Default
                    && battleResult.LevelNum == dataProvider.Levels.Count))
            {
                postBattleScreen.completedGameMessage.SetActive(true);
            }
            else
            {
                postBattleScreen.appraisalSection.Initialise(levelTrashTalkData.AppraisalDroneText, soundPlayer, ShowLoot);
                postBattleScreen.lootAcquiredText.SetActive(true);
                _unlockedLoot = lootManager.UnlockLoot(battleResult.LevelNum);
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