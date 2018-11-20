using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps
{
    public interface ITutorialStepArgsNEW
    {
        IHighlighterNEW Highlighter { get; }
        string TextToDisplay { get; }
        ITextDisplayer Displayer { get; }
		IItemProvider<IMaskHighlightable> HighlightableProvider { get; }
    }
}
