namespace BattleCruisers.Tutorial.Highlighting
{
    public interface ICoreHighlighter
    {
        void Highlight(HighlightArgs args);
        void Unhighlight();
    }
}