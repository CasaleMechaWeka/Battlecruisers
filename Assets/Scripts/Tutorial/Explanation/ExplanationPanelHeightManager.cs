using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationPanelHeightManager
    {
        private readonly ExplanationPanel _explanationPanel;

        private const float SHRUNK_CHARACTER_COUNT = 38;


        public ExplanationPanelHeightManager(ExplanationPanel explanationPanel)
        {
            Helper.AssertIsNotNull(explanationPanel);

            _explanationPanel = explanationPanel;

            _explanationPanel.DoneButton.EnabledChange += UpdatePanelHeight;
            _explanationPanel.OkButton.EnabledChange += UpdatePanelHeight;
            _explanationPanel.TextDisplayer.TextChanged += UpdatePanelHeight;
        }

        private void UpdatePanelHeight(object sender, EventArgs args)
        {
            Logging.Log(Tags.TUTORIAL_EXPLANATION_PANEL, $"sender: {sender}");

            if (!_explanationPanel.DoneButton.Enabled
                && !_explanationPanel.OkButton.Enabled
                && _explanationPanel.TextDisplayer.Text.Length < SHRUNK_CHARACTER_COUNT)
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