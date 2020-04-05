using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Explanation
{
    // FELIX  Test
    public class ExplanationPanelHeightManager
    {
        private readonly IExplanationPanel _explanationPanel;

        private const float SHRUNK_CHARACTER_COUNT = 50;

        public ExplanationPanelHeightManager(IExplanationPanel explanationPanel)
        {
            Assert.IsNotNull(explanationPanel);

            _explanationPanel = explanationPanel;

            _explanationPanel.DoneButton.EnabledChange += UpdatePanelHeight;
            _explanationPanel.OkButton.EnabledChange += UpdatePanelHeight;
            _explanationPanel.TextDisplayer.TextChanged += UpdatePanelHeight;
        }

        private void UpdatePanelHeight(object sender, EventArgs args)
        {
            if (CanShrinkPanel())
            {
                _explanationPanel.Shrink();
            }
            else
            {
                _explanationPanel.Expand();
            }
        }

        // FELIX  Abstract?
        private bool CanShrinkPanel()
        {
            return
                !_explanationPanel.DoneButton.Enabled
                && !_explanationPanel.DoneButton.Enabled
                && _explanationPanel.TextDisplayer.Text.Length < SHRUNK_CHARACTER_COUNT;
        }
    }
}