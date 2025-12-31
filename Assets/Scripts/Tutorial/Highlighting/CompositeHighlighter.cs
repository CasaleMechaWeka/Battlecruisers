using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class CompositeHighlighter : ICoreHighlighter
    {
        private readonly ICoreHighlighter[] _highlighters;

        public CompositeHighlighter(params ICoreHighlighter[] highlighters)
        {
            Assert.IsTrue(highlighters.Length > 0);
            _highlighters = highlighters;
        }

        public void Highlight(HighlightArgs args)
        {
            foreach (ICoreHighlighter highlighter in _highlighters)
            {
                highlighter.Highlight(args);
            }
        }

        public void Unhighlight()
        {
            foreach (ICoreHighlighter highlighter in _highlighters)
            {
                highlighter.Unhighlight();
            }
        }
    }
}