using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Providers;

namespace BattleCruisers.Tutorial.Steps
{
    public interface ITutorialStepArgs
    {
        IHighlighter Highlighter { get; }
        string TextToDisplay { get; }
        ITextDisplayer Displayer { get; }
		IHighlightablesProvider HighlightablesProvider { get; }
    }
}
