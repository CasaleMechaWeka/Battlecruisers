using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.Filters;
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
        private IPermitter _navigationPermitter;

        [SetUp]
        public void TestSetup()
        {
            _pauseGameManager = Substitute.For<IPauseGameManager>();
            _modalMenu = Substitute.For<IModalMenu>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();
            _sceneNavigator = Substitute.For<ISceneNavigator>();
            _navigationPermitter = Substitute.For<IPermitter>();

            _mainMenuManager = new MainMenuManager(_pauseGameManager, _modalMenu, _battleCompletionHandler, _sceneNavigator, _navigationPermitter);
        }

        [Test]
        public void ShowMenu()
        {
            _mainMenuManager.ShowMenu();

            _pauseGameManager.Received().PauseGame();
            _navigationPermitter.Received().IsMatch = false;
            _modalMenu.Received().ShowMenu();
        }

        [Test]
        public void DismissMenu()
        {
            _mainMenuManager.DismissMenu();

            _pauseGameManager.Received().ResumeGame();
            _navigationPermitter.Received().IsMatch = true;
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