using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgs : ITutorialArgsBase
    {
        ExplanationPanel ExplanationPanel { get; }
        IButton ModalMainMenuButton { get; }
    }
}
