namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface IZoomCalculator
    {
        /// <returns>
        /// The orthographic size change to add or subtract to the camera's orthographic
        /// size due to the mouse scroll delta.
        /// </returns>
        float FindZoomDelta(float mouseScrollDeltaY);
    }
}