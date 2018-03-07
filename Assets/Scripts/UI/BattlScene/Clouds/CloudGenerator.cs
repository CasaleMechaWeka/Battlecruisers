using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudGenerator : ICloudGenerator
    {
        private readonly ICloudFactory _cloudFactory;

        public CloudGenerator(ICloudFactory cloudFactory)
        {
            Assert.IsNotNull(cloudFactory);
            _cloudFactory = cloudFactory;
        }

        public void GenerateClouds(ICloudGenerationStats generationStats)
        {
            float totalArea = generationStats.CloudSpawnArea.width * generationStats.CloudSpawnArea.height;
            float targetArea = totalArea * generationStats.CloudDensityAsFraction;
            float areaUsed = 0;

            ICloudStats cloudStats = new CloudStats(generationStats);

            while (areaUsed < targetArea)
            {
                Vector2 spawnPosition = FindSpawnPosition(generationStats.CloudSpawnArea);
                ICloud newCloud = _cloudFactory.CreateCloud(spawnPosition);
                newCloud.Initialise(cloudStats);

                float cloudArea = newCloud.Size.x * newCloud.Size.y;
                areaUsed += cloudArea;
            }
        }

        private Vector2 FindSpawnPosition(Rect spawnArea)
        {
            float xPos = Random.Range(spawnArea.xMin, spawnArea.xMax);
            float yPos = Random.Range(spawnArea.yMin, spawnArea.yMax);

            return new Vector2(xPos, yPos);
        }
    }
}
