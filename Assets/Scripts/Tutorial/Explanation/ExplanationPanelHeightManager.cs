using BattleCruisers.Utils;
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
            Logging.Log(Tags.TUTORIAL_EXPLANATION_PANEL, $"sender: {sender}");

            if (CanShrinkPanel())
            {
                _explanationPanel.ShrinkHeight();
            }
            else
            {
                _explanationPanel.ExpandHeight();
            }
        }

        // FELIX  Abstract?
        private bool CanShrinkPanel()
        {
            bool result =
                !_explanationPanel.DoneButton.Enabled
                && !_explanationPanel.OkButton.Enabled
                && _explanationPanel.TextDisplayer.Text.Length < SHRUNK_CHARACTER_COUNT;

            Logging.Log(Tags.TUTORIAL_EXPLANATION_PANEL, $"Result: {result}  Done button: {_explanationPanel.DoneButton.Enabled}  Ok button: {_explanationPanel.OkButton.Enabled}  Text length: {_explanationPanel.TextDisplayer.Text.Length}");

            return result;
        }
    }
}