namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public enum CameraCorner
    {
        PlayerCruiser,  // Bottom left corner
        Overview,       // Top center corner
        AICruiser       // Bottom right corner
    }

    public interface ICornerIdentifier
    {
        /// <returns>
        /// The camera corner the given cameraTarget is in, or null if
        /// the cameraTarget is not in any corner (ie, in the center ish).
        /// </returns>
        CameraCorner? FindCorner(ICameraTarget cameraTarget);
    }
}