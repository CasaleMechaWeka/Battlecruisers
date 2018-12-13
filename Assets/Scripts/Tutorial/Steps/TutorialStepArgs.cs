using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps
{
    public class TutorialStepArgs : ITutorialStepArgs
    {
        public IHighlighter Highlighter { get; private set; }
        public string TextToDisplay { get; private set; }
        public ITextDisplayer Displayer { get; private set; }
		public IItemProvider<IMaskHighlightable> HighlightableProvider { get; private set; }

        public TutorialStepArgs(
            IHighlighter highlighter,
            string textToDisplay,
            ITextDisplayer displayer,
            IItemProvider<IMaskHighlightable> highlightableProvider)
        {
            // textToDisplay may be null
            Helper.AssertIsNotNull(highlighter, displayer, highlightableProvider);

            Highlighter = highlighter;
            TextToDisplay = textToDisplay;
            Displayer = displayer;
            HighlightableProvider = highlightableProvider;
        }
    }
}
