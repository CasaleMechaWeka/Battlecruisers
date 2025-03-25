using BattleCruisers.Data;
using BattleCruisers.Data.Models;

namespace BattleCruisers.UI.ScreensScene.CoinBattleScreen
{
    public class CoinBattleScreenController : ScreenController
    {
        public void Battle()
        {
            ApplicationModel.Mode = GameMode.CoinBattle;
            SaveCoinBattleSettings();

            int maxLevel = DataProvider.GameModel.NumOfLevelsCompleted; //might need null or not-0 check?
            int levelIndex = UnityEngine.Random.Range(1, maxLevel);
            _screensSceneGod.GoToTrashScreen(levelIndex);
        }

        private void SaveCoinBattleSettings()
        {
            DataProvider.GameModel.CoinBattle
                = new CoinBattleModel(
                    DataProvider.SettingsManager.AIDifficulty,
                    DataProvider.GameModel.PlayerLoadout.Hull
                    );
            DataProvider.SaveGame();
        }

        public void Home()
        {
            _screensSceneGod.GotoHubScreen();
        }
    }
}
