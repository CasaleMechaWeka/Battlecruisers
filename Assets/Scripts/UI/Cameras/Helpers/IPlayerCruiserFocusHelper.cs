namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface IPlayerCruiserFocusHelper
    {
        /// <summary>
        /// Focuses in the player cruiser if the camera is not already
        /// roughly on the player cruiser.
        /// </summary>
        void FocusOnPlayerCruiserIfNeeded();
    }
}