using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.UI.BattleScene
{
    public class MainMenuManager : IMainMenuManager
    {
        private readonly IPauseGameManager _pauseGameManager;
        private readonly IModalMenu _modalMenu;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly ISceneNavigator _sceneNavigator;
        private readonly INavigationPermitterManager _navigationPermitterManager;

        private NavigationPermittersState _stateOnShowMenu;

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
            _stateOnShowMenu = _navigationPermitterManager.PauseNavigation();
            _pauseGameManager.PauseGame();
            _modalMenu.ShowMenu();
        }

        public void DismissMenu()
        {
            if (_stateOnShowMenu != null)
            {
                _navigationPermitterManager.RestoreNavigation(_stateOnShowMenu);
            }
            _pauseGameManager.ResumeGame();
            _modalMenu.HideMenu();
        }

        public void QuitGame()
        {
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false);
            _modalMenu.HideMenu();
        }

        public void RetryLevel()
        {
            // Need to resume game to get music back
            _pauseGameManager.ResumeGame();
            _sceneNavigator.GoToScene(SceneNames.BATTLE_SCENE);
        }
    }
}