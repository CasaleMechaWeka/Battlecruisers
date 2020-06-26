using BattleCruisers.Tutorial.Explanation;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.InGameHints
{
    public class HintDisplayer : IHintDisplayer
    {
        private readonly IExplanationPanel _explanationPanel;

        public HintDisplayer(IExplanationPanel explanationPanel)
        {
            Assert.IsNotNull(explanationPanel);

            _explanationPanel = explanationPanel;
            _explanationPanel.OkButton.Clicked += OkButton_Clicked;
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            _explanationPanel.IsVisible = false;
        }

        public void ShowHint(string hint)
        {
            Assert.IsFalse(string.IsNullOrEmpty(hint));

            _explanationPanel.IsVisible = true;
            _explanationPanel.TextDisplayer.DisplayText(hint);
            _explanationPanel.OkButton.Enabled = true;
        }
    }
}