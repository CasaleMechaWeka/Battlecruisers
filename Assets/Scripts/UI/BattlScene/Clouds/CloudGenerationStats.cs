using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudGenerationStats : ICloudGenerationStats
    {
        /// <summary>
        /// For example, to put clouds in this area:
        /// 
        /// x: -80 -> 80
        /// y:  25 -> 60
        /// 
        /// Create Rect like this:
        /// 
        /// Rect cloudArea = new Rect(-80, 25, 160, 35);
        /// </summary>
        public Rect CloudSpawnArea { get; private set; }

        public float CloudDensityAsFraction { get; private set; }
        public float CloudHorizontalMovementSpeedInS { get; private set; }

        public CloudGenerationStats(
            Rect cloudSpawnArea,
            CloudDensity density = CloudDensity.Medium,
            CloudMovementSpeed movementSpeed = CloudMovementSpeed.Slow)
        {
            CloudSpawnArea = cloudSpawnArea;
            CloudDensityAsFraction = ConvertCloudDensity(density);
            CloudHorizontalMovementSpeedInS = ConvertMovementSpeed(movementSpeed);
        }

        private float ConvertCloudDensity(CloudDensity density)
        {
            switch (density)
            {
                case CloudDensity.High:
                    return 0.5f;
                case CloudDensity.Medium:
                    return 0.25f;
                case CloudDensity.Low:
                default:
                    return 0.1f;
            }
        }

        private float ConvertMovementSpeed(CloudMovementSpeed movementSpeed)
        {
            switch (movementSpeed)
            {
                case CloudMovementSpeed.Fast:
                    return 1;
                case CloudMovementSpeed.Slow:
                default:
                    return 0.5f;
            }
        }
    }
}
