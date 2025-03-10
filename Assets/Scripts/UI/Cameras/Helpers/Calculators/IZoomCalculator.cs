namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public interface IZoomCalculator
    {
        /// <returns>
        /// The orthographic size change to add or subtract to the camera's orthographic
        /// size due to the mouse scroll delta.
        /// </returns>
        float FindMouseScrollOrthographicSizeDelta(float mouseScrollDeltaY);

        /// <returns>
        /// The orthographic size change to add or subtract to the camera's orthographic
        /// size due to the pinch zoom delta.
        /// </returns>
        float FindPinchZoomOrthographicSizeDelta(float pinchZoomDelta);
    }
}