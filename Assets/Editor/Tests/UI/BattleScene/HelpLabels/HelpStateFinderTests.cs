using BattleCruisers.UI.BattleScene.HelpLabels.States;
using BattleCruisers.UI.Panels;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.HelpLabels
{
    public class HelpStateFinderTests
    {
        private IHelpStateFinder _helpStateFinder;
        private ISlidingPanel _informatorPanel, _selectorPanel;
        private IHelpState _bothCollapsed, _selectorShown, _informatorShown, _bothShown;

        [SetUp]
        public void TestSetup()
        {
            _informatorPanel = Substitute.For<ISlidingPanel>();
            _selectorPanel = Substitute.For<ISlidingPanel>();
            _bothCollapsed = Substitute.For<IHelpState>();
            _selectorShown = Substitute.For<IHelpState>();
            _informatorShown = Substitute.For<IHelpState>();
            _bothShown = Substitute.For<IHelpState>();

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

            IHelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_bothShown, result);
        }

        [Test]
        public void FindHelpState_InformatorShown()
        {
            _informatorPanel.TargetState.Returns(PanelState.Shown);
            _selectorPanel.TargetState.Returns(PanelState.Hidden);

            IHelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_informatorShown, result);
        }

        [Test]
        public void FindHelpState_SelectorShown()
        {
            _informatorPanel.TargetState.Returns(PanelState.Hidden);
            _selectorPanel.TargetState.Returns(PanelState.Shown);

            IHelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_selectorShown, result);
        }

        [Test]
        public void FindHelpState_BothCollapsed()
        {
            _informatorPanel.TargetState.Returns(PanelState.Hidden);
            _selectorPanel.TargetState.Returns(PanelState.Hidden);

            IHelpState result = _helpStateFinder.FindHelpState();

            Assert.AreSame(_bothCollapsed, result);
        }
    }
}