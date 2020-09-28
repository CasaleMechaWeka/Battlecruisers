using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Threading.Tasks;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen.States
{
    public class VictoryState
    {
        private const string VICTORY_TITLE = "Sweet as!";

        public async Task InitialiseAsync(
            PostBattleScreenController postBattleScreen,
            IApplicationModel appModel,
            ISingleSoundPlayer soundPlayer,
            IMusicPlayer musicPlayer,
            IDataProvider dataProvider,
            IDifficultySpritesProvider difficultySpritesProvider,
            ILootManager lootManager)
        {
            Helper.AssertIsNotNull(postBattleScreen, appModel, soundPlayer, musicPlayer, dataProvider, difficultySpritesProvider, lootManager);

            BattleResult battleResult = dataProvider.GameModel.LastBattleResult;

            postBattleScreen.title.text = VICTORY_TITLE;
            musicPlayer.PlayVictoryMusic();
            await postBattleScreen.completedDifficultySymbol.InitialiseAsync(dataProvider.SettingsManager.AIDifficulty, difficultySpritesProvider);
            postBattleScreen.completedDifficultySymbol.gameObject.SetActive(true);

            if (dataProvider.StaticData.IsDemo
                && battleResult.LevelNum == StaticData.NUM_OF_LEVELS_IN_DEMO)
            {
                postBattleScreen.demoCompletedScreen.SetActive(true);
                postBattleScreen.demoHomeButton.Initialise(soundPlayer, postBattleScreen.GoToHomeScreen);
            }

            if (lootManager.ShouldShowLoot(battleResult.LevelNum))
            {
                postBattleScreen.lootAcquiredText.SetActive(true);
                postBattleScreen.unlockedItemSection.Show();

                lootManager.UnlockLoot(battleResult.LevelNum);
            }
            else if (battleResult.LevelNum == dataProvider.Levels.Count
                && battleResult.LevelNum > dataProvider.GameModel.NumOfLevelsCompleted)
            {
                // Completed last level for the frist time
                postBattleScreen.completedGameMessage.SetActive(true);
            }
            else
            {
                postBattleScreen.victoryNoLootMessage.SetActive(true);
            }

            CompletedLevel level = new CompletedLevel(levelNum: battleResult.LevelNum, hardestDifficulty: dataProvider.SettingsManager.AIDifficulty);
            dataProvider.GameModel.AddCompletedLevel(level);
            dataProvider.SaveGame();
        }
    }
}