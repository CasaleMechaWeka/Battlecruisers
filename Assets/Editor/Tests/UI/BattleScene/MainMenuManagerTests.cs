using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene
{
    public class MainMenuManagerTests
    {
        private IMainMenuManager _mainMenuManager;
        private IPauseGameManager _pauseGameManager;
        private IModalMenu _modalMenu;
        private IBattleCompletionHandler _battleCompletionHandler;
        private ISceneNavigator _sceneNavigator;
        private INavigationPermitterManager _navigationPermitterManager;
        private NavigationPermittersState _preMenuState;

        [SetUp]
        public void TestSetup()
        {
            _pauseGameManager = Substitute.For<IPauseGameManager>();
            _modalMenu = Substitute.For<IModalMenu>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();
            _sceneNavigator = Substitute.For<ISceneNavigator>();
            _navigationPermitterManager = Substitute.For<INavigationPermitterManager>();

            _mainMenuManager = new MainMenuManager(_pauseGameManager, _modalMenu, _battleCompletionHandler, _sceneNavigator, _navigationPermitterManager);

            _preMenuState = new NavigationPermittersState(default, default, default, default);
            _navigationPermitterManager.PauseNavigation().Returns(_preMenuState);
        }

        [Test]
        public void ShowMenu()
        {
            _mainMenuManager.ShowMenu();

            _navigationPermitterManager.Received().PauseNavigation();
            _pauseGameManager.Received().PauseGame();
            _modalMenu.Received().ShowMenu();
        }

        [Test]
        public void ShowMenu_SecondTme()
        {
            _mainMenuManager.ShowMenu();
            _pauseGameManager.ClearReceivedCalls();
            _navigationPermitterManager.ClearReceivedCalls();
            _modalMenu.ClearReceivedCalls();

            _mainMenuManager.ShowMenu();

            _navigationPermitterManager.DidNotReceive().PauseNavigation();
            _pauseGameManager.Received().PauseGame();
            _modalMenu.Received().ShowMenu();
        }

        [Test]
        public void DismissMenu_WithouPreviousShow()
        {
            _mainMenuManager.DismissMenu();

            _navigationPermitterManager.DidNotReceiveWithAnyArgs().RestoreNavigation(default);
            _pauseGameManager.Received().ResumeGame();
            _modalMenu.Received().HideMenu();
        }

        [Test]
        public void DismissMenu_WithPreviousShow()
        {
            _mainMenuManager.ShowMenu();

            _mainMenuManager.DismissMenu();

            _navigationPermitterManager.RestoreNavigation(_preMenuState);
            _pauseGameManager.Received().ResumeGame();
            _modalMenu.Received().HideMenu();
        }

        [Test]
        public void QuitGame()
        {
            _mainMenuManager.QuitGame();

            _pauseGameManager.Received().ResumeGame();
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false);
            _modalMenu.Received().HideMenu();
        }

        [Test]
        public void RetryLevel()
        {
            _mainMenuManager.RetryLevel();

            _pauseGameManager.Received().ResumeGame();
            _sceneNavigator.Received().GoToScene(SceneNames.BATTLE_SCENE);
        }
    }
}