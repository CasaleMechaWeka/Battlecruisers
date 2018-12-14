using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ITutorialStepArgsFactory
    {
        ITutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay = null,
            IMaskHighlightable highlightable = null);

        ITutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay,
            IItemProvider<IMaskHighlightable> highlightableProvider);
    }
}