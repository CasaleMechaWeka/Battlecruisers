using BattleCruisers.UI;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Explanation
{
    public class HeightDecider : IHeightDecider
    {
        private const float SHRUNK_CHARACTER_COUNT = 50;

        public bool CanShrinkPanel(ITogglable doneButton, ITogglable okButton, string text)
        {
            bool result =
                !doneButton.Enabled
                && !okButton.Enabled
                && text.Length < SHRUNK_CHARACTER_COUNT;

            Logging.Log(Tags.TUTORIAL_EXPLANATION_PANEL, $"Result: {result}  Done button: {doneButton.Enabled}  Ok button: {okButton.Enabled}  Text length: {text.Length}");

            return result;
        }
    }
}