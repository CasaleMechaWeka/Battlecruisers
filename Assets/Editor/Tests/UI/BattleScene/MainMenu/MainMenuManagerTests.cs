using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.MainMenu
{
    public class MainMenuManagerTests : ModalManagerTestsBase
    {
        private IMainMenuManager _mainMenuManager;
        private IModalMenu _modalMenu;
        private IBattleCompletionHandler _battleCompletionHandler;
        private int _dismissedCount;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _modalMenu = Substitute.For<IModalMenu>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();

            _mainMenuManager = new MainMenuManager(_navigationPermitterManager, _pauseGameManager, _modalMenu, _battleCompletionHandler);

            _dismissedCount = 0;
            _mainMenuManager.Dismissed += (sender, e) => _dismissedCount++;
        }

        [Test]
        public void ShowMenu()
        {
            _mainMenuManager.ShowMenu();
            _modalMenu.Received().ShowMenu();
        }

        [Test]
        public void DismissMenu()
        {
            _mainMenuManager.DismissMenu();

            _modalMenu.Received().HideMenu();
            Assert.AreEqual(1, _dismissedCount);
        }

        [Test]
        public void QuitGame()
        {
            _mainMenuManager.QuitGame();

            _pauseGameManager.Received().ResumeGame();
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false, retryLevel: false);
            _modalMenu.DidNotReceive().HideMenu();
            Assert.AreEqual(1, _dismissedCount);
        }

        [Test]
        public void RetryLevel()
        {
            _mainMenuManager.RetryLevel();

            _pauseGameManager.Received().ResumeGame();
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false, retryLevel: true);
            _modalMenu.DidNotReceive().HideMenu();
            Assert.AreEqual(1, _dismissedCount);
        }
    }
}