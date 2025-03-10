namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface IEdgeDetector
    {
        bool IsCursorAtLeftEdge();
        bool IsCursorAtRightEdge();
    }
}