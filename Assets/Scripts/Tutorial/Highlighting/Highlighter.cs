using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class Highlighter
    {
        private readonly ICoreHighlighter _coreHighlighter;
        private readonly HighlightArgsFactory _highlightArgsFactory;

        public Highlighter(ICoreHighlighter coreHighlighter, HighlightArgsFactory highlightArgsFactory)
        {
            Helper.AssertIsNotNull(coreHighlighter, highlightArgsFactory);

            _coreHighlighter = coreHighlighter;
            _highlightArgsFactory = highlightArgsFactory;
        }

        public void Highlight(IHighlightable toHighlight)
        {
            Assert.IsNotNull(toHighlight);

            HighlightArgs highlightArgs = toHighlight.CreateHighlightArgs(_highlightArgsFactory);
            _coreHighlighter.Highlight(highlightArgs);
        }

        public void Unhighlight()
        {
            _coreHighlighter.Unhighlight();
        }
    }
}
