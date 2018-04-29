using UnityEngine;

namespace BattleCruisers.Tutorial
{
    public interface IHighlighter
    {
        void Highlight(params IHighlightable[] toHighlight);
        void UnhighlightAll();
    }
}
