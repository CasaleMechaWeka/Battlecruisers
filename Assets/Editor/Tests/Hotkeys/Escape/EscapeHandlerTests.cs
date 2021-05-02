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
        private IHelpLabelManager _helpLabelManager;

        [SetUp]
        public void TestSetup()
        {
            _escapeDetector = Substitute.For<IEscapeDetector>();
            _mainMenuManager = Substitute.For<IMainMenuManager>();
            _helpLabelManager = Substitute.For<IHelpLabelManager>();

            _escapeHandler = new EscapeHandler(_escapeDetector, _mainMenuManager, _helpLabelManager);
        }

        [Test]
        public void HelpLabelsShown()
        {
            _helpLabelManager.IsShown.Value.Returns(true);
            _escapeDetector.EscapePressed += Raise.Event();
            _helpLabelManager.Received().HideHelpLabels();
        }

        [Test]
        public void MainMenuShown()
        {
            _helpLabelManager.IsShown.Value.Returns(false);
            _mainMenuManager.IsShown.Returns(true);

            _escapeDetector.EscapePressed += Raise.Event();

            _mainMenuManager.Received().DismissMenu();
        }

        [Test]
        public void NeitherShown()
        {
            _helpLabelManager.IsShown.Value.Returns(false);
            _mainMenuManager.IsShown.Returns(false);

            _escapeDetector.EscapePressed += Raise.Event();

            _mainMenuManager.Received().ShowMenu();
        }
    }
}