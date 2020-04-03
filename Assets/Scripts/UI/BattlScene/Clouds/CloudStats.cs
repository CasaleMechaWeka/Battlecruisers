using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudStats : ICloudStats
    {
        public float HorizontalMovementSpeedInMPerS { get; }
        public Color FrontCloudColour { get; }
        public Color BackCloudColour { get; }

        public CloudStats(
            float horizontalMovementSpeedInMPerS,
            Color frontCloudColour,
            Color backCloudColour)
        {
            HorizontalMovementSpeedInMPerS = horizontalMovementSpeedInMPerS;
            FrontCloudColour = frontCloudColour;
            BackCloudColour = backCloudColour;
        }
    }
}
