using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class TutorialStepArgsFactory
    {
        private readonly Highlighter _highlighter;
        private readonly ITextDisplayer _displayer;

        public TutorialStepArgsFactory(Highlighter highlighter, ITextDisplayer displayer)
        {
            Helper.AssertIsNotNull(highlighter, displayer);

            _highlighter = highlighter;
            _displayer = displayer;
        }

        public TutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay = null,
            IHighlightable highlightable = null,
            bool shouldUnhighlight = true)
        {
            return
                CreateTutorialStepArgs(
                    textToDisplay,
                    new StaticProvider<IHighlightable>(highlightable),
                    shouldUnhighlight);
        }

        public TutorialStepArgs CreateTutorialStepArgs(
            string textToDisplay,
            IItemProvider<IHighlightable> highlightableProvider,
            bool shouldUnhighlight = true)
        {
            Assert.IsNotNull(highlightableProvider);

            return
                new TutorialStepArgs(
                    _highlighter,
                    textToDisplay,
                    _displayer,
                    highlightableProvider,
                    shouldUnhighlight);
        }
    }
}