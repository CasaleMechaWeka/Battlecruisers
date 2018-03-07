using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudStats : ICloudStats
    {
        public float HorizontalMovementSpeedInMPerS { get; private set; }
        public float DisappearLineInM { get; private set; }
        public float ReappaerLineInM { get; private set; }

        public CloudStats(ICloudGenerationStats generationStats)
        {
            Assert.IsNotNull(generationStats);

            HorizontalMovementSpeedInMPerS = generationStats.CloudHorizontalMovementSpeedInS;

            if (HorizontalMovementSpeedInMPerS > 0)
            {
                // Left to right
                ReappaerLineInM = generationStats.CloudSpawnArea.xMin;
                DisappearLineInM = generationStats.CloudSpawnArea.xMax;
            }
            else
            {
                // Right to left
                DisappearLineInM = generationStats.CloudSpawnArea.xMin;
                ReappaerLineInM = generationStats.CloudSpawnArea.xMax;
            }
        }
    }
}
