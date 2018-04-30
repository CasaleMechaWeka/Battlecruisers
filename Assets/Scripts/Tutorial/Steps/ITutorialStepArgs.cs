using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Tutorial.Steps
{
    public interface ITutorialStepArgs
    {
        IHighlightable[] ElementsToHighlight { get; }
        IHighlighter Highlighter { get; }
        string TextToDisplay { get; }
        ITextDisplayer Displayer { get; }
    }
}
