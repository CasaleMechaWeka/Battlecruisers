using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlighter
    {
        void Highlight(IList<IHighlightable> toHighlight);
        void UnhighlightAll();
    }
}
