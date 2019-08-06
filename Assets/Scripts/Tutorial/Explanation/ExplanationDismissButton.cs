using BattleCruisers.UI;
using System;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationDismissButton : CanvasGroupButton, IExplanationDismissButton
    {
        public event EventHandler Clicked;

        protected override void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}