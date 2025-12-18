using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenController : ScreenController
    {
        private BattleResult _lastBattleResult;

        public void Initialise(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer)
        {
            base.Initialise(screensSceneGod);

            _lastBattleResult = DataProvider.GameModel.LastBattleResult;

            HomeScreenLayout layout = GetLayout(DataProvider.GameModel);
            layout.Initialise(this, DataProvider.GameModel, soundPlayer);
            layout.IsVisible = true;
        }



        private HomeScreenLayout GetLayout(GameModel gameModel)
        {
            // TEMP  Force layouyts :)
            //return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeLayout");
            //return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeNonTutorial");
            //return transform.FindNamedComponent<HomeScreenLayout>("DefaultLayout");

            if (!gameModel.HasAttemptedTutorial)
            {
                // First time play
                return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeLayout");
            }
            else if (gameModel.FirstNonTutorialBattle)
            {
                // First time playing non-tutorial
                return transform.FindNamedComponent<HomeScreenLayout>("FirstTimeNonTutorial");
            }
            else
            {
                // Normal play
                return transform.FindNamedComponent<HomeScreenLayout>("DefaultLayout");
            }
        }

        public void Continue()
        {
            Assert.IsNotNull(_lastBattleResult);

            int nextLevelToPlay = DataProvider.GameModel.NumOfLevelsCompleted < 31 ? DataProvider.GameModel.NumOfLevelsCompleted + 1 : 1;
            _screensSceneGod.GoToTrashScreen(nextLevelToPlay);
        }
        public void StartBattleHub()
        {
            _screensSceneGod.GotoHubScreen();
        }

        public void GoToLevelsScreen()
        {
            _screensSceneGod.GoToLevelsScreen();
        }

        public void GoToMultiplayScreen()
        {
            _screensSceneGod.LoadPvPBattleScene();
        }

        public void GoToLoadoutScreen()
        {
            _screensSceneGod.GoToLoadoutScreen();
        }

        public void GoToSettingsScreen()
        {
            _screensSceneGod.GoToSettingsScreen();
        }

        public void StartTutorial()
        {
            ApplicationModel.Mode = GameMode.Tutorial;
            _screensSceneGod.GoToTrashScreen(levelNum: 1);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void GoToChooseDifficultyScreen()
        {
            _screensSceneGod.GoToChooseDifficultyScreen();
        }

        public void GoToSkirmishScreen()
        {
            _screensSceneGod.GoToSkirmishScreen();
        }

        public void ShowNewsPanel()
        {
            _screensSceneGod.ShowNewsPanel();
        }

        /// <summary>
        /// Manual cloud save - saves current game state to cloud
        /// </summary>
        public async void CloudSave()
        {
            try
            {
                await DataProvider.CloudSave();
                Debug.Log("[HomeScreen] Cloud save completed successfully");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[HomeScreen] Cloud save failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Manual cloud load - loads game state from cloud (overwrites local if cloud is newer)
        /// </summary>
        public async void CloudLoad()
        {
            try
            {
                await DataProvider.CloudLoad();
                Debug.Log("[HomeScreen] Cloud load completed successfully");
                // Note: UI will refresh automatically when navigating to other screens
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[HomeScreen] Cloud load failed: {ex.Message}");
            }
        }
    }
}
