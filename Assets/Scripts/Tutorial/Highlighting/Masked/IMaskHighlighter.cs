namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public interface IMaskHighlighter
    {
        void Highlight(HighlightArgs args);
        void Unhighlight();
    }
}