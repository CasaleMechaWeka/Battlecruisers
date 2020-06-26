using BattleCruisers.Tutorial.Explanation;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgs : ITutorialArgsBase
    {
        ExplanationPanel ExplanationPanel { get; }
    }
}
