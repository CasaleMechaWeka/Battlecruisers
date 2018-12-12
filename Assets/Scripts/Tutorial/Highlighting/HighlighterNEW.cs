using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class HighlighterNEW : IHighlighterNEW
    {
        private readonly IMaskHighlighter _maskHighlighter;
        private readonly IHighlightArgsFactory _highlightArgsFactory;

        public HighlighterNEW(IMaskHighlighter maskHighlighter, IHighlightArgsFactory highlightArgsFactory)
        {
            Helper.AssertIsNotNull(maskHighlighter, highlightArgsFactory);

            _maskHighlighter = maskHighlighter;
            _highlightArgsFactory = highlightArgsFactory;
        }

        public void Highlight(IMaskHighlightable toHighlight)
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
