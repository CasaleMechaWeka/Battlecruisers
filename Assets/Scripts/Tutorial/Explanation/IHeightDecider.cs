using BattleCruisers.UI;

namespace BattleCruisers.Tutorial.Explanation
{
    public interface IHeightDecider
    {
        bool CanShrinkPanel(ITogglable doneButton, ITogglable okButton, string text);
    }
}