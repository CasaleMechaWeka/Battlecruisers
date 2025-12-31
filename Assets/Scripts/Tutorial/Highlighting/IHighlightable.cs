namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlightable
    {
        HighlightArgs CreateHighlightArgs(HighlightArgsFactory highlightArgsFactory);
    }
}