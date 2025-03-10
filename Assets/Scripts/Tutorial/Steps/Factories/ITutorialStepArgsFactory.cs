using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface ITutorialStepArgsFactory
    {
        ITutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay = null,
            IHighlightable highlightable = null,
            bool shouldUnhighlight = true);

        ITutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay,
            IItemProvider<IHighlightable> highlightableProvider,
            bool shouldUnhighlight = true);
    }
}