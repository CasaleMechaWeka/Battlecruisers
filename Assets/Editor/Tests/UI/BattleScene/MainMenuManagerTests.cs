using BattleCruisers.Scenes;
using BattleCruisers.UI.BattleScene;
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

        [SetUp]
        public void TestSetup()
        {
            _pauseGameManager = Substitute.For<IPauseGameManager>();
            _modalMenu = Substitute.For<IModalMenu>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();
            _sceneNavigator = Substitute.For<ISceneNavigator>();

            _mainMenuManager = new MainMenuManager(_pauseGameManager, _modalMenu, _battleCompletionHandler, _sceneNavigator);
        }

        [Test]
        public void ShowMenu()
        {
            _mainMenuManager.ShowMenu();

            _pauseGameManager.Received().PauseGame();
            _modalMenu.Received().ShowMenu(_mainMenuManager);
        }

        [Test]
        public void DismissMenu()
        {
            _mainMenuManager.DismissMenu();

            _pauseGameManager.Received().ResumeGame();
            _modalMenu.Received().HideMenu();
        }

        [Test]
        public void QuitGame()
        {
            _mainMenuManager.QuitGame();

            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false);
            _modalMenu.Received().HideMenu();
        }

        [Test]
        public void RetryLevel()
        {
            _mainMenuManager.RetryLevel();
            _sceneNavigator.Received().GoToScene(SceneNames.BATTLE_SCENE);
        }
    }
}