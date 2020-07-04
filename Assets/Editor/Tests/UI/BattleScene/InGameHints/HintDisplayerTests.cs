using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI.BattleScene.InGameHints;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class HintDisplayerTests
    {
        private IHintDisplayer _hintDisplayer;
        private IExplanationPanel _explanationPanel;
        private string _hint1, _hint2;

        [SetUp]
        public void TestSetup()
        {
            _explanationPanel = Substitute.For<IExplanationPanel>();
            _hintDisplayer = new HintDisplayer(_explanationPanel);

            _hint1 = "monkas";
            _hint2 = "elafantos";
        }

        [Test]
        public void ShowHint()
        {
            _hintDisplayer.ShowHint(_hint1);

            _explanationPanel.Received().IsVisible = true;
            _explanationPanel.TextDisplayer.Received().DisplayText(_hint1);
            _explanationPanel.OkButton.Received().Enabled = true;
        }

        [Test]
        public void OkButtonClicked()
        {
            _explanationPanel.OkButton.Clicked += Raise.Event();
            _explanationPanel.Received().IsVisible = false;
        }

        [Test]
        public void HideHint_NotVisible()
        {
            _explanationPanel.IsVisible.Returns(false);
            _explanationPanel.TextDisplayer.Text.Returns(_hint1);

            _hintDisplayer.HideHint(_hint1);

            _explanationPanel.DidNotReceive().IsVisible = false;
        }

        [Test]
        public void HideHint_Visible_NonMatchingHint()
        {
            _explanationPanel.IsVisible.Returns(true);
            _explanationPanel.TextDisplayer.Text.Returns(_hint2);

            _hintDisplayer.HideHint(_hint1);

            _explanationPanel.DidNotReceive().IsVisible = false;
        }

        [Test]
        public void HideHint_Visible_MatchingHint()
        {
            _explanationPanel.IsVisible.Returns(true);
            _explanationPanel.TextDisplayer.Text.Returns(_hint1);

            _hintDisplayer.HideHint(_hint1);

            _explanationPanel.Received().IsVisible = false;
        }
    }
}