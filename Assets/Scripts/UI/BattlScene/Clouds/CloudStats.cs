using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudStats : ICloudStats
    {
        public float HorizontalMovementSpeedInMPerS { get; }
        public float DisappearLineInM { get; }
        public float ReappaerLineInM { get; }

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
