namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public interface IMaskHighlightable
    {
        HighlightArgs CreateHighlightArgs(IHighlightArgsFactory highlightArgsFactory);
    }
}