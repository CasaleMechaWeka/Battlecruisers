using BattleCruisers.Hotkeys.Escape;
using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.BattleScene.MainMenu;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Hotkeys.Escape
{
    public class EscapeHandlerTests
    {
        private EscapeHandler _escapeHandler;
        private IEscapeDetector _escapeDetector;
        private IMainMenuManager _mainMenuManager;

        [SetUp]
        public void TestSetup()
        {
            _escapeDetector = Substitute.For<IEscapeDetector>();
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