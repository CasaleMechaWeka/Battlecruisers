using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class CompositeHighlighter : IMaskHighlighter
    {
        private readonly IMaskHighlighter[] _highlighters;

        public CompositeHighlighter(params IMaskHighlighter[] highlighters)
        {
            Assert.IsTrue(highlighters.Length > 0);
            _highlighters = highlighters;
        }

        public void Highlight(HighlightArgs args)
        {
            foreach (IMaskHighlighter highlighter in _highlighters)
            {
                highlighter.Highlight(args);
            }
        }

        public void Unhighlight()
        {
            foreach (IMaskHighlighter highlighter in _highlighters)
            {
                highlighter.Unhighlight();
            }
        }
    }
}