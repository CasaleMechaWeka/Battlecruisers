namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public class CornerCutoffProvider : ICornerCutoffProvider
    {
        // With an aspect ratio of 4:3:
        private const float DEFAULT_ASPECT_RATIO = 1.333f;
        // The smallest x-value I can get to via the navigation wheel is:  -37.4
        private const float DEFAULT_PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF = -35;
        // The largest x-value I can get to via the navigation wheel is:  37.4
        public const float DEFAULT_AI_CRUISER_CORNER_X_POSITION_CUTOFF = 35;
        // The largest orthographic size I can get to via the navigation wheel is 32.7
        public const float DEFAULT_OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF = 29;

        public float PlayerCruiserCornerXPositionCutoff { get; private set; }
        public float AICruiserCornerXPositionCutoff { get; private set; }
        public float OverviewOrthographicSizeCutoff { get; private set; }

        public CornerCutoffProvider(float cameraAspectRatio)
        {
            float adjustmentMultiplier = cameraAspectRatio / DEFAULT_ASPECT_RATIO;

            PlayerCruiserCornerXPositionCutoff = DEFAULT_PLAYER_CRUISER_CORNER_X_POSITION_CUTOFF * adjustmentMultiplier;
            AICruiserCornerXPositionCutoff = DEFAULT_AI_CRUISER_CORNER_X_POSITION_CUTOFF * adjustmentMultiplier;

            // Because we scale the UI via height (keep height ratios constant), the
            // orthographic size cutoff is the same regardless of aspect ratio.
            OverviewOrthographicSizeCutoff = DEFAULT_OVERVIEW_ORTHOGRAPHIC_SIZE_CUTOFF;
        }
    }
}