using BattleCruisers.UI.BattleScene.HelpLabels;
using BattleCruisers.UI.BattleScene.HelpLabels.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.HelpLabels
{
    public class HelpLabelManagerTests : ModalManagerTestsBase
    {
        private IHelpLabelManager _helpLabelManager;
        private IHelpStateFinder _helpStateFinder;
        private IHelpState _helpState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _helpStateFinder = Substitute.For<IHelpStateFinder>();
            _helpLabelManager = new HelpLabelManager(_navigationPermitterManager, _pauseGameManager, _helpStateFinder);

            _helpState = Substitute.For<IHelpState>();
            _helpStateFinder.FindHelpState().Returns(_helpState);
        }

        [Test]
        public void ShowHelpLabels_FirstTime()
        {
            _helpLabelManager.ShowHelpLabels();

            _helpState.Received().ShowHelpLabels();
            Assert.IsTrue(_helpLabelManager.IsShown.Value);
        }

        [Test]
        public void ShowHelpLabels_SecondTime()
        {
            _helpLabelManager.ShowHelpLabels();
            _helpState.ClearReceivedCalls();

            _helpLabelManager.ShowHelpLabels();

            _helpState.DidNotReceive().ShowHelpLabels();
            Assert.IsTrue(_helpLabelManager.IsShown.Value);
        }

        [Test]
        public void HideHelpLabels_NoPreviousShow()
        {
            _helpLabelManager.HideHelpLabels();
            _helpState.DidNotReceive().HideHelpLables();
        }

        [Test]
        public void HideHelpLabels_WithPreviousShow()
        {
            _helpLabelManager.ShowHelpLabels();

            _helpLabelManager.HideHelpLabels();

            _helpState.Received().HideHelpLables();
        }

        [Test]
        public void SecondHideHelpLabels_WithPreviousShow()
        {
            _helpLabelManager.ShowHelpLabels();
            _helpLabelManager.HideHelpLabels();
            _helpState.ClearReceivedCalls();

            _helpLabelManager.HideHelpLabels();

            _helpState.DidNotReceive().HideHelpLables();
        }
    }
}