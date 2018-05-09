using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps
{
    public class TutorialStepArgs : ITutorialStepArgs
    {
        public IHighlighter Highlighter { get; private set; }
        public string TextToDisplay { get; private set; }
        public ITextDisplayer Displayer { get; private set; }
        public IListProvider<IHighlightable> HighlightablesProvider { get; private set; }

        public TutorialStepArgs(
            IHighlighter highlighter,
            string textToDisplay,
            ITextDisplayer displayer,
            IListProvider<IHighlightable> highlightablesProvider)
        {
            Helper.AssertIsNotNull(highlighter, displayer, highlightablesProvider);

            Highlighter = highlighter;
            TextToDisplay = textToDisplay;
            Displayer = displayer;
            HighlightablesProvider = highlightablesProvider;
        }
    }
}
