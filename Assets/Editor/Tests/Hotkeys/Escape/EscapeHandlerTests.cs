using BattleCruisers.Hotkeys;
using BattleCruisers.UI.BattleScene.MainMenu;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Hotkeys.Escape
{
    public class EscapeHandlerTests
    {
        private EscapeHandler _escapeHandler;
        private EscapeDetector _escapeDetector;
        private IMainMenuManager _mainMenuManager;

        [SetUp]
        public void TestSetup()
        {
            _escapeDetector = Substitute.For<EscapeDetector>();
            _mainMenuManager = Substitute.For<IMainMenuManager>();

            _escapeHandler = new EscapeHandler(_escapeDetector, _mainMenuManager);
        }

        [Test]
        public void MainMenuShown()
        {

            _mainMenuManager.IsShown.Returns(true);

            _escapeDetector.EscapePressed += Raise.Event();

            _mainMenuManager.Received().DismissMenu();
        }

        [Test]
        public void NeitherShown()
        {

            _mainMenuManager.IsShown.Returns(false);

            _escapeDetector.EscapePressed += Raise.Event();

            _mainMenuManager.Received().ShowMenu();
        }
    }
}