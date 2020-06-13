using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.BattleScene
{
    public class MainMenuManager : IMainMenuManager
    {
        private readonly IPauseGameManager _pauseGameManager;
        private readonly IModalMenu _modalMenu;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly ISceneNavigator _sceneNavigator;

        public MainMenuManager(
            IPauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            IBattleCompletionHandler battleCompletionHandler,
            ISceneNavigator sceneNavigator)
        {
            Helper.AssertIsNotNull(pauseGameManager, modalMenu, battleCompletionHandler, sceneNavigator);

            _pauseGameManager = pauseGameManager;
            _modalMenu = modalMenu;
            _battleCompletionHandler = battleCompletionHandler;
            _sceneNavigator = sceneNavigator;
        }

        public void ShowMenu()
        {
            _pauseGameManager.PauseGame();
            _modalMenu.ShowMenu(this);
        }

        public void DismissMenu()
        {
            _pauseGameManager.ResumeGame();
            _modalMenu.HideMenu();
        }

        public void QuitGame()
        {
            _battleCompletionHandler.CompleteBattle(wasVictory: false);
            _modalMenu.HideMenu();
        }

        public void RetryLevel()
        {
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
        }
    }
}