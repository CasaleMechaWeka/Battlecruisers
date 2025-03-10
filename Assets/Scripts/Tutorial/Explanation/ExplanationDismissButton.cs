using BattleCruisers.UI;

namespace BattleCruisers.Tutorial.Explanation
{
    public class ExplanationDismissButton : CanvasGroupButton, IExplanationDismissButton
    {
        protected override bool ToggleVisibility => true;
    }
}