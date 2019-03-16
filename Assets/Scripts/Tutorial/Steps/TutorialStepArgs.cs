using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps
{
    public class TutorialStepArgs : ITutorialStepArgs
    {
        public IHighlighter Highlighter { get; }
        public string TextToDisplay { get; }
        public ITextDisplayer Displayer { get; }
		public IItemProvider<IMaskHighlightable> HighlightableProvider { get; }

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
