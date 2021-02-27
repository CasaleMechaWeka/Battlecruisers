using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgs : ITutorialArgsBase
    {
        ExplanationPanel ExplanationPanel { get; }
        IButton ModalMainMenuButton { get; }
        ILocTable TutorialStrings { get; }
    }
}
