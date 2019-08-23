using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudGenerator : ICloudGenerator
    {
        private readonly ICloudFactory _cloudFactory;
        private readonly IRandomGenerator _random;

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
                Vector3 spawnPosition = FindSpawnPosition(generationStats.CloudSpawnArea, generationStats.ZPositionRange);
                ICloud newCloud = _cloudFactory.CreateCloud(spawnPosition);
                newCloud.Initialise(cloudStats);

                float cloudArea = newCloud.Size.x * newCloud.Size.y;
                areaUsed += cloudArea;
            }
        }

        private Vector3 FindSpawnPosition(Rect spawnArea, IRange<float> zPositionRange)
        {
            float xPos = _random.Range(spawnArea.xMin, spawnArea.xMax);
            float yPos = _random.Range(spawnArea.yMin, spawnArea.yMax);
            float zPos = _random.Range(zPositionRange.Min, zPositionRange.Max);

            return new Vector3(xPos, yPos, zPos);
        }
    }
}
