using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using System;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class MainMenuManager : IMainMenuManager
    {
        private readonly IPauseGameManager _pauseGameManager;
        private readonly IModalMenu _modalMenu;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly ISceneNavigator _sceneNavigator;
        private readonly INavigationPermitterManager _navigationPermitterManager;

        private NavigationPermittersState _stateOnShowMenu;

        public event EventHandler Dismissed;

        public MainMenuManager(
            IPauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            IBattleCompletionHandler battleCompletionHandler,
            ISceneNavigator sceneNavigator,
            INavigationPermitterManager navigationPermitterManager)
        {
            Helper.AssertIsNotNull(pauseGameManager, modalMenu, battleCompletionHandler, sceneNavigator, navigationPermitterManager);

            _pauseGameManager = pauseGameManager;
            _modalMenu = modalMenu;
            _battleCompletionHandler = battleCompletionHandler;
            _sceneNavigator = sceneNavigator;
            _navigationPermitterManager = navigationPermitterManager;
        }

        public void ShowMenu()
        {
            if (_stateOnShowMenu == null)
            {
                _stateOnShowMenu = _navigationPermitterManager.PauseNavigation();
            }
            _pauseGameManager.PauseGame();
            _modalMenu.ShowMenu();
        }

        public void DismissMenu()
        {
            if (_stateOnShowMenu != null)
            {
                _navigationPermitterManager.RestoreNavigation(_stateOnShowMenu);
                _stateOnShowMenu = null;
            }
            _pauseGameManager.ResumeGame();
            _modalMenu.HideMenu();
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void QuitGame()
        {
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false);
            _modalMenu.HideMenu();
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void RetryLevel()
        {
            // FELIX  Check this works for skirmish :)
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
            Dismissed?.Invoke(this, EventArgs.Empty);
        }

        public void ShowSettings()
        {
            _modalMenu.ShowSettings();
        }
    }
}