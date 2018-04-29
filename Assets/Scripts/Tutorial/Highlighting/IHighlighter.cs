using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlighter
    {
        void Highlight(params IHighlightable[] toHighlight);
        void UnhighlightAll();
    }
}
