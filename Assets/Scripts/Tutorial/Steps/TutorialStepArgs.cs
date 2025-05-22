using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps
{
    public class TutorialStepArgs : ITutorialStepArgs
    {
        public Highlighter Highlighter { get; }
        public string TextToDisplay { get; }
        public ITextDisplayer Displayer { get; }
        public IItemProvider<IHighlightable> HighlightableProvider { get; }
        public bool ShouldUnhighlight { get; }

        public TutorialStepArgs(
            Highlighter highlighter,
            string textToDisplay,
            ITextDisplayer displayer,
            IItemProvider<IHighlightable> highlightableProvider,
            bool shouldUnhighlight)
        {
            // textToDisplay may be null
            Helper.AssertIsNotNull(highlighter, displayer, highlightableProvider);

            Highlighter = highlighter;
            TextToDisplay = textToDisplay;
            Displayer = displayer;
            HighlightableProvider = highlightableProvider;
            ShouldUnhighlight = shouldUnhighlight;
        }
    }
}
