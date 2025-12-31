using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.UI.Panels;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.HelpLabels
{
    public class HelpStateFinderTests
    {
        private HelpStateFinder _helpStateFinder;
        private SlidingPanel _informatorPanel, _selectorPanel;
        private HelpState _bothCollapsed, _selectorShown, _informatorShown, _bothShown;

        [SetUp]
        public void TestSetup()
        {
            _informatorPanel = Substitute.For<SlidingPanel>();
            _selectorPanel = Substitute.For<SlidingPanel>();
            _bothCollapsed = Substitute.For<HelpState>();
            _selectorShown = Substitute.For<HelpState>();
            _informatorShown = Substitute.For<HelpState>();
            _bothShown = Substitute.For<HelpState>();

            _helpStateFinder
                = new HelpStateFinder(
                    _informatorPanel,
                    _selectorPanel,
                    _bothCollapsed,
                    _selectorShown,
                    _informatorShown,
                    _bothShown);
        }

        [Test]
        public void FindHelpState_BothShown()
        {
            _informatorPanel.TargetState.Returns(PanelState.Shown);
            _selectorPanel.TargetState.Returns(PanelState.Shown);

            HelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_bothShown, result);
        }

        [Test]
        public void FindHelpState_InformatorShown()
        {
            _informatorPanel.TargetState.Returns(PanelState.Shown);
            _selectorPanel.TargetState.Returns(PanelState.Hidden);

            HelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_informatorShown, result);
        }

        [Test]
        public void FindHelpState_SelectorShown()
        {
            _informatorPanel.TargetState.Returns(PanelState.Hidden);
            _selectorPanel.TargetState.Returns(PanelState.Shown);

            HelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_selectorShown, result);
        }

        [Test]
        public void FindHelpState_BothCollapsed()
        {
            _informatorPanel.TargetState.Returns(PanelState.Hidden);
            _selectorPanel.TargetState.Returns(PanelState.Hidden);

            HelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_bothCollapsed, result);
        }
    }
}