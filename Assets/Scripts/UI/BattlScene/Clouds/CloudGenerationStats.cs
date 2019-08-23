using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

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
        public Rect CloudSpawnArea { get; }

        public float CloudDensityAsFraction { get; }
        public float CloudHorizontalMovementSpeedInS { get; }
        public IRange<float> ZPositionRange { get; }

        public CloudGenerationStats(
            Rect cloudSpawnArea,
            IRange<float> zPositionRange,
            CloudDensity density = CloudDensity.Medium,
            CloudMovementSpeed movementSpeed = CloudMovementSpeed.Slow)
        {
            Assert.IsNotNull(zPositionRange);

            CloudSpawnArea = cloudSpawnArea;
            ZPositionRange = zPositionRange;
            CloudDensityAsFraction = ConvertCloudDensity(density);
            CloudHorizontalMovementSpeedInS = ConvertMovementSpeed(movementSpeed);
        }

        private float ConvertCloudDensity(CloudDensity density)
        {
            switch (density)
            {
                case CloudDensity.VeryHigh:
                    return 4;
                case CloudDensity.High:
                    return 2;
                case CloudDensity.Medium:
                    return 1.5f;
                case CloudDensity.Low:
                default:
                    return 0.75f;
            }
        }

        private float ConvertMovementSpeed(CloudMovementSpeed movementSpeed)
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
