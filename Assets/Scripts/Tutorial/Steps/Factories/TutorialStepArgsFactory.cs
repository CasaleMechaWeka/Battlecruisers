using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class TutorialStepArgsFactory : ITutorialStepArgsFactory
    {
        private readonly IHighlighter _highlighter;
        private readonly ITextDisplayer _displayer;

        public TutorialStepArgsFactory(IHighlighter highlighter, ITextDisplayer displayer)
        {
            Helper.AssertIsNotNull(highlighter, displayer);

            _highlighter = highlighter;
            _displayer = displayer;
        }

        public ITutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay = null,
            IHighlightable highlightable = null)
        {
            return CreateTutorialStepArgs(textToDisplay, new StaticProvider<IHighlightable>(highlightable));
        }

        public ITutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay,
            IItemProvider<IHighlightable> highlightableProvider)
        {
            Assert.IsNotNull(highlightableProvider);

            return
                new TutorialStepArgs(
                    _highlighter,
                    textToDisplay,
                    _displayer,
                    highlightableProvider);
        }
    }
}