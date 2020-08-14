using BattleCruisers.Scenes;
using BattleCruisers.UI.Filters;
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
        private readonly IPermitter _navigationPermitter;

        public MainMenuManager(
            IPauseGameManager pauseGameManager,
            IModalMenu modalMenu,
            IBattleCompletionHandler battleCompletionHandler,
            ISceneNavigator sceneNavigator,
            IPermitter navigationPermitter)
        {
            Helper.AssertIsNotNull(pauseGameManager, modalMenu, battleCompletionHandler, sceneNavigator, navigationPermitter);

            _pauseGameManager = pauseGameManager;
            _modalMenu = modalMenu;
            _battleCompletionHandler = battleCompletionHandler;
            _sceneNavigator = sceneNavigator;
            _navigationPermitter = navigationPermitter;
        }

        public void ShowMenu()
        {
            _pauseGameManager.PauseGame();
            _navigationPermitter.IsMatch = false;
            _modalMenu.ShowMenu();
        }

        public void DismissMenu()
        {
            _pauseGameManager.ResumeGame();
            _navigationPermitter.IsMatch = true;
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