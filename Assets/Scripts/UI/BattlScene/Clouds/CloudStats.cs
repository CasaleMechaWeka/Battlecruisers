using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudStats : ICloudStats
    {
        public float HorizontalMovementSpeedInMPerS { get; }
        public Color FrontCloudColour { get; }
        public Color BackCloudColour { get; }

        // FELIX Remove?
        public CloudStats(
            CloudMovementSpeed cloudMovementSpeed,
            Color frontCloudColour,
            Color backCloudColour)
            : this(ConvertMovementSpeed(cloudMovementSpeed), frontCloudColour, backCloudColour)
        {
        }

        public CloudStats(
            float horizontalMovementSpeedInMPerS,
            Color frontCloudColour,
            Color backCloudColour)
        {
            HorizontalMovementSpeedInMPerS = horizontalMovementSpeedInMPerS;
            FrontCloudColour = frontCloudColour;
            BackCloudColour = backCloudColour;
        }

        // FELIX Remove?
        private static float ConvertMovementSpeed(CloudMovementSpeed movementSpeed)
        {
            switch (movementSpeed)
            {
                case CloudMovementSpeed.Fast:
                    return 0.75f;
                case CloudMovementSpeed.Slow:
                default:
                    return 0.5f;
            }
        }
    }
}
