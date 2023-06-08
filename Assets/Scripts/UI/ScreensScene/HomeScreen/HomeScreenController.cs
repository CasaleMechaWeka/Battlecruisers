using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenController : ScreenController, IHomeScreen
    {
        private BattleResult _lastBattleResult;
        private INextLevelHelper _nextLevelHelper;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, nextLevelHelper);

            _lastBattleResult = dataProvider.GameModel.LastBattleResult;
            _nextLevelHelper = nextLevelHelper;

            HomeScreenLayout layout = GetLayout(dataProvider.GameModel);
            layout.Initialise(this, dataProvider.GameModel, soundPlayer);
            layout.IsVisible = true;
        }

        private HomeScreenLayout GetLayout(IGameModel gameModel)
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

            int nextLevelToPlay = _nextLevelHelper.FindNextLevel();
            _screensSceneGod.GoToTrashScreen(nextLevelToPlay);
        }


        public void StartBattleHub()
        {

        }

        public void GoToLevelsScreen()
        {
            _screensSceneGod.GoToLevelsScreen();
        }

        public void GoToMultiplayScreen()
        {
            _screensSceneGod.LoadMultiplayScene();
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
            ApplicationModelProvider.ApplicationModel.Mode = GameMode.Tutorial;
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
    }
}
