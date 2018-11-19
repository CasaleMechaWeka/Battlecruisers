namespace BattleCruisers.UI.Cameras.Adjusters
{
    public interface ICameraAdjuster
    {
        /// <returns>
        /// True if the camera is now on target, false otherwise.  False
        /// indicates that further camera adjustments are needed to reach
        /// the camera target.
        /// </returns>
        bool AdjustCamera();
    }
}