using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps
{
    public class TutorialStepArgs : ITutorialStepArgs
    {
        public IHighlighter Highlighter { get; private set; }
        public string TextToDisplay { get; private set; }
        public ITextDisplayer Displayer { get; private set; }
        public IHighlightablesProvider HighlightablesProvider { get; private set; }

        public TutorialStepArgs(
            IHighlighter highlighter,
            string textToDisplay,
            ITextDisplayer displayer,
            IHighlightablesProvider highlightablesProvider)
        {
            Helper.AssertIsNotNull(highlighter, displayer, highlightablesProvider);

            Highlighter = highlighter;
            TextToDisplay = textToDisplay;
            Displayer = displayer;
            HighlightablesProvider = highlightablesProvider;
        }
    }
}
