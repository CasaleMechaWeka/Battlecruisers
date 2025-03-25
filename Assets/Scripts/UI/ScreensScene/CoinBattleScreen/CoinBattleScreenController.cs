using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.CoinBattleScreen
{
    public class CoinBattleScreenController : ScreenController
    {
        private IApplicationModel _applicationModel;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            IApplicationModel applicationModel)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(applicationModel);

            _applicationModel = applicationModel;
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.CoinBattle;
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
