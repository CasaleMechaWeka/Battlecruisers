using BattleCruisers.Tutorial.Explanation;
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
        }

        public void ShowHint(string hint)
        {
            Assert.IsFalse(string.IsNullOrEmpty(hint));

            _explanationPanel.TextDisplayer.DisplayText(hint);
            _explanationPanel.OkButton.Enabled = true;
        }
    }
}