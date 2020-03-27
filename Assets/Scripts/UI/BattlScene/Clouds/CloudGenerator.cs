using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudGenerator : ICloudGenerator
    {
        private readonly ICloudFactory _cloudFactory;
        private readonly IRandomGenerator _random;
        public const float CLOUD_Z_POSITION = 10;

        public CloudGenerator(ICloudFactory cloudFactory, IRandomGenerator random)
        {
            Helper.AssertIsNotNull(cloudFactory, random);

            _cloudFactory = cloudFactory;
            _random = random;
        }

        public void GenerateClouds(ICloudGenerationStats generationStats)
        {
            float totalArea = generationStats.CloudSpawnArea.width * generationStats.CloudSpawnArea.height;
            float targetArea = totalArea * generationStats.CloudDensityAsFraction;
            float areaUsed = 0;

            ICloudStats cloudStats = _cloudFactory.CreateCloudStats(generationStats);

            while (areaUsed < targetArea)
            {
                Vector3 spawnPosition = FindSpawnPosition(generationStats.CloudSpawnArea);
                ICloud newCloud = _cloudFactory.CreateCloud(spawnPosition);
                newCloud.Initialise(cloudStats);

                float cloudArea = newCloud.Size.x * newCloud.Size.y;
                areaUsed += cloudArea;
            }
        }

        private Vector3 FindSpawnPosition(Rect spawnArea)
        {
            float xPos = _random.Range(spawnArea.xMin, spawnArea.xMax);
            float yPos = _random.Range(spawnArea.yMin, spawnArea.yMax);

            return new Vector3(xPos, yPos, CLOUD_Z_POSITION);
        }
    }
}
