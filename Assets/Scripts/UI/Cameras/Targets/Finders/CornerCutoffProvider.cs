namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public class CornerCutoffProvider : ICornerCutoffProvider
    {
        // With an aspect ratio of 4:3 and a maximum orthographic size of 38 (35):
        private const float DEFAULT_ASPECT_RATIO = 1.333f;
        // The smallest x-value I can get to via the navigation wheel is:  -44 (-37.4)
        private const float DEFAULT_PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF = -40;
        // The largest x-value I can get to via the navigation wheel is:  44 (37.4)
        public const float DEFAULT_AI_CRUISER_CORNER_X_POSITION_CUTOFF = 40;
        // The largest orthographic size I can get to via the navigation wheel is 37.6 (32.7)
        public const float DEFAULT_OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF = 34;

        public float PlayerCruiserCornerXPositionCutoff { get; }
        public float AICruiserCornerXPositionCutoff { get; }
        public float OverviewOrthographicSizeCutoff { get; }

        public CornerCutoffProvider(float cameraAspectRatio)
        {
            float adjustmentMultiplier = cameraAspectRatio / DEFAULT_ASPECT_RATIO;

            PlayerCruiserCornerXPositionCutoff = DEFAULT_PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF / adjustmentMultiplier;
            AICruiserCornerXPositionCutoff = DEFAULT_AI_CRUISER_CORNER_X_POSITION_CUTOFF / adjustmentMultiplier;
            OverviewOrthographicSizeCutoff = DEFAULT_OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF / adjustmentMultiplier;
        }
    }
}