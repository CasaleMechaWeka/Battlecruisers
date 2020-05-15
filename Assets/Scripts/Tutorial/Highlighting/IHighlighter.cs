namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlighter
    {
        void Highlight(IHighlightable toHighlight);
        void Unhighlight();
    }
}
