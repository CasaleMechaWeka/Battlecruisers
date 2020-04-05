using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Tutorial.Explanation
{
    // FELIX  Test
    public class ExplanationPanelHeightManager
    {
        private readonly IExplanationPanel _explanationPanel;
        private readonly IHeightDecider _heightDecider;

        private const float SHRUNK_CHARACTER_COUNT = 50;

        public ExplanationPanelHeightManager(IExplanationPanel explanationPanel, IHeightDecider heightDecider)
        {
            Helper.AssertIsNotNull(explanationPanel, heightDecider);

            _explanationPanel = explanationPanel;
            _heightDecider = heightDecider;

            _explanationPanel.DoneButton.EnabledChange += UpdatePanelHeight;
            _explanationPanel.OkButton.EnabledChange += UpdatePanelHeight;
            _explanationPanel.TextDisplayer.TextChanged += UpdatePanelHeight;
        }

        private void UpdatePanelHeight(object sender, EventArgs args)
        {
            Logging.Log(Tags.TUTORIAL_EXPLANATION_PANEL, $"sender: {sender}");

            if (_heightDecider.CanShrinkPanel(_explanationPanel.DoneButton, _explanationPanel.OkButton, _explanationPanel.TextDisplayer.Text))
            {
                _explanationPanel.ShrinkHeight();
            }
            else
            {
                _explanationPanel.ExpandHeight();
            }
        }
    }
}