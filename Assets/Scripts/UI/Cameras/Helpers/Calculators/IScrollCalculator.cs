namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public interface IScrollCalculator
    {
        float FindScrollDelta(float swipeDeltaX);
        float FindZoomDelta(float swipeDeltaY);
    }
}