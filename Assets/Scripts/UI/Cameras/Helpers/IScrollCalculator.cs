namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface IScrollCalculator
    {
        float FindScrollDelta(float swipeDeltaX);
        float FindZoomDelta(float swipeDeltaY);
    }
}