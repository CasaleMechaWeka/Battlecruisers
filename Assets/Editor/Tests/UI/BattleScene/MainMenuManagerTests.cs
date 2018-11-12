using BattleCruisers.UI.BattleScene;
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
        private MenuDismissed _menuDismissedCallback;

        [SetUp]
        public void TestSetup()
        {
            _pauseGameManager = Substitute.For<IPauseGameManager>();
            _modalMenu = Substitute.For<IModalMenu>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();

            _mainMenuManager = new MainMenuManager(_pauseGameManager, _modalMenu, _battleCompletionHandler);

            _modalMenu.ShowMenu(Arg.Do<MenuDismissed>(callback => _menuDismissedCallback = callback));
        }

        [Test]
        public void ShowMenu()
        {
            _mainMenuManager.ShowMenu();

            _pauseGameManager.Received().PauseGame();
            _modalMenu.Received().ShowMenu(Arg.Any<MenuDismissed>());
            Assert.IsNotNull(_menuDismissedCallback);
        }

        [Test]
        public void OnModalMenuDismissed_UserAction_Dismissed()
        {
            ShowMenu();

            _menuDismissedCallback.Invoke(UserAction.Dismissed);

            _pauseGameManager.Received().ResumeGame();
        }

        [Test]
        public void OnModalMenuDismissed_UserAction_Quit()
        {
            ShowMenu();

            _menuDismissedCallback.Invoke(UserAction.Quit);

            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false);
        }
    }
}