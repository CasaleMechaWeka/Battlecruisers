using BattleCruisers.Tutorial.Highlighting;

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
