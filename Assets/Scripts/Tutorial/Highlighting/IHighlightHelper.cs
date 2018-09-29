namespace BattleCruisers.Tutorial.Highlighting
{
    public interface IHighlightHelper
    {
        IHighlight CreateHighlight(IHighlightable highlightable, bool usePulsingAnimation = true);
    }
}
