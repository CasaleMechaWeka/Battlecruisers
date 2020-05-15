using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class Highlighter : IHighlighter
    {
        private readonly ICoreHighlighter _maskHighlighter;
        private readonly IHighlightArgsFactory _highlightArgsFactory;

        public Highlighter(ICoreHighlighter maskHighlighter, IHighlightArgsFactory highlightArgsFactory)
        {
            Helper.AssertIsNotNull(maskHighlighter, highlightArgsFactory);

            _maskHighlighter = maskHighlighter;
            _highlightArgsFactory = highlightArgsFactory;
        }

        public void Highlight(IHighlightable toHighlight)
        {
            Assert.IsNotNull(toHighlight);

            HighlightArgs highlightArgs = toHighlight.CreateHighlightArgs(_highlightArgsFactory);
            _maskHighlighter.Highlight(highlightArgs);
        }

        public void Unhighlight()
        {
            _maskHighlighter.Unhighlight();
        }
    }
}
