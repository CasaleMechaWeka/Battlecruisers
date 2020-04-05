using BattleCruisers.Tutorial.Explanation;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Explanation
{
    public class ExplanationPanelHeightManagerTests
    {
        private ExplanationPanelHeightManager _heightManager;
        private IExplanationPanel _panel;
        private IHeightDecider _decider;

        [SetUp]
        public void TestSetup()
        {
            _panel = Substitute.For<IExplanationPanel>();
            _decider = Substitute.For<IHeightDecider>();

            _heightManager = new ExplanationPanelHeightManager(_panel, _decider);

            _panel.TextDisplayer.Text.Returns("Sweet text :D");
            _decider.CanShrinkPanel(_panel.DoneButton, _panel.OkButton, _panel.TextDisplayer.Text).Returns(false);
        }

        [Test]
        public void UpdatePanelHeight_TriggeredByDoneButton_CanShrinkFalse()
        {
            _panel.DoneButton.EnabledChange += Raise.Event();
            _panel.Received().ExpandHeight();
        }

        [Test]
        public void UpdatePanelHeight_TriggeredByOkButton_CanShrinkFalse()
        {
            _panel.OkButton.EnabledChange += Raise.Event();
            _panel.Received().ExpandHeight();
        }

        [Test]
        public void UpdatePanelHeight_TriggeredByText_CanShrinkFalse()
        {
            _panel.TextDisplayer.TextChanged += Raise.Event();
            _panel.Received().ExpandHeight();
        }

        [Test]
        public void UpdatePanelHeight_CanShrinkTrue()
        {
            _decider.CanShrinkPanel(_panel.DoneButton, _panel.OkButton, _panel.TextDisplayer.Text).Returns(true);
            _panel.DoneButton.EnabledChange += Raise.Event();
            _panel.Received().ShrinkHeight();
        }
    }
}