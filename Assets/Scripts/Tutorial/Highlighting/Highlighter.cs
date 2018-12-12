using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    // NEWUI  Remove, tests have been converted :P
    public class Highlighter : IHighlighter
    {
        private readonly IHighlightHelper _highlightHelper;
        private readonly IList<IHighlight> _highlights;

        public Highlighter(IHighlightHelper highlightHelper)
        {
            Assert.IsNotNull(highlightHelper);

            _highlightHelper = highlightHelper;
            _highlights = new List<IHighlight>();
        }

        public void Highlight(IList<IHighlightable> toHighlight)
        {
            Assert.IsTrue(_highlights.Count == 0, "Should only highlight group of IHighlightables at a time.");

            foreach (IHighlightable highlightable in toHighlight)
            {
                _highlights.Add(_highlightHelper.CreateHighlight(highlightable));
            }
        }

        public void UnhighlightAll()
        {
            foreach (IHighlight highlight in _highlights)
            {
                highlight.Destroy();
            }

            _highlights.Clear();
        }
    }
}
