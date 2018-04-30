using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps
{
    public class TutorialStepArgs : ITutorialStepArgs
    {
        public IHighlightable[] ElementsToHighlight { get; private set; }
        public IHighlighter Highlighter { get; private set; }
        public string TextToDisplay { get; private set; }
        public ITextDisplayer Displayer { get; private set; }

        public TutorialStepArgs(
            IHighlighter highlighter,
            string textToDisplay,
            ITextDisplayer displayer,
            params IHighlightable[] elementsToHighlight)
        {
            Helper.AssertIsNotNull(highlighter, displayer, elementsToHighlight);

            ElementsToHighlight = elementsToHighlight;
            Highlighter = highlighter;
            TextToDisplay = textToDisplay;
            Displayer = displayer;
        }
    }
}
