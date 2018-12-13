using BattleCruisers.Tutorial.Highlighting.Masked;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlighter
    {
        void Highlight(IMaskHighlightable toHighlight);
        void Unhighlight();
    }
}
