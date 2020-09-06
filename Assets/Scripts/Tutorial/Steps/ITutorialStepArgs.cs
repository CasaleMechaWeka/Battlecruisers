using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;

namespace BattleCruisers.Tutorial.Steps
{
    public interface ITutorialStepArgs
    {
        IHighlighter Highlighter { get; }
        string TextToDisplay { get; }
        ITextDisplayer Displayer { get; }
		IItemProvider<IHighlightable> HighlightableProvider { get; }
        bool ShouldUnhighlight { get; }
    }
}
