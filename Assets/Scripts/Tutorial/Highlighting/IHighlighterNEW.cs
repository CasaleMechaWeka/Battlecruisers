using BattleCruisers.Tutorial.Highlighting.Masked;

namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlighterNEW
    {
        void Highlight(IMaskHighlightable toHighlight);
        void Unhighlight();
    }
}
