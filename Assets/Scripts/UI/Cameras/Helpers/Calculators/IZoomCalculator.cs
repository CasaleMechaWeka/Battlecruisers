namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public interface IZoomCalculator
    {
        /// <returns>
        /// The orthographic size change to add or subtract to the camera's orthographic
        /// size due to the mouse scroll delta.
        /// </returns>
        /// FELIX  Rename :)
        float FindZoomDelta(float mouseScrollDeltaY);
    }
}