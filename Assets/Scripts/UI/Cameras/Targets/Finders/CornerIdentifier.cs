using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    // FELIX  Test :D
    public class CornerIdentifier : ICornerIdentifier
    {
        // The smallest x-value I can get to via the navigation wheel is:  -37.4
        public const float PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF = -35;
        
        // The largest x-value I can get to via the navigation wheel is:  37.4
        public const float AI_CRUISER_CORNER_X_POSITION_CUTOFF = 35;

        // The largest orthographic size I can get to via the navigation wheel is 32.7
        public const float OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF = 29;

        public CameraCorner? FindCorner(ICameraTarget cameraTarget)
        {
            Assert.IsNotNull(cameraTarget);

            if (cameraTarget.Position.x <= PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF)
            {
                return CameraCorner.PlayerCruiser;
            }
            else if (cameraTarget.Position.x >= AI_CRUISER_CORNER_X_POSITION_CUTOFF)
            {
                return CameraCorner.AICruiser;
            }
            else if (cameraTarget.OrthographicSize >= OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF)
            {
                return CameraCorner.Overview;
            }
            else
            {
                return null;
            }
        }
    }
}