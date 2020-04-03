using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds
{
    public class CloudGeneratorTests
    {
        private ICloudGenerator _generator;

        private ICloudFactory _factory;
        private ICloudGenerationStats _generationStats;
        private ICloudStatsExtended _cloudStats;
        private ICloud _cloud;

        [SetUp]
        public void SetuUp()
        {
            // -10 <= x <= 10
            //  5 <= y <= 10
            // Area = 100
            Rect cloudSpawnArea = new Rect(x: -10, y: 5, width: 20, height: 5);
            IRange<float> zPositionRange = new Range<float>(-5, 5);

            _generationStats = Substitute.For<ICloudGenerationStats>();
            _generationStats.CloudSpawnArea.Returns(cloudSpawnArea);
            _generationStats.CloudDensityAsFraction.Returns(0.5f);

            _cloud = Substitute.For<ICloud>();
            Vector2 cloudSize = new Vector2(1, 1);  // Area = 1
            _cloud.Size.Returns(cloudSize);

			_cloudStats = Substitute.For<ICloudStatsExtended>();

            _factory = Substitute.For<ICloudFactory>();
            _factory.CreateCloudStats(_generationStats).Returns(_cloudStats);
            _factory.CreateCloud(default).ReturnsForAnyArgs(_cloud);
			
			_generator = new CloudGenerator(_factory, RandomGenerator.Instance);
        }

        [Test]
        public void GenerateClouds()
        {
            _generator.GenerateClouds(_generationStats);

            _factory.Received().CreateCloudStats(_generationStats);

			// Target area = 50
			// Area per cloud = 1
			// => Should generate 50 clouds :)
            int expectedNumOfClouds = 50;
			
            _factory.Received(expectedNumOfClouds).CreateCloud(Arg.Is<Vector3>(
                spawnPosition =>
                    spawnPosition.x >= _generationStats.CloudSpawnArea.xMin
                    && spawnPosition.x <= _generationStats.CloudSpawnArea.xMax
                    && spawnPosition.y >= _generationStats.CloudSpawnArea.yMin
                    && spawnPosition.y <= _generationStats.CloudSpawnArea.yMax
                    && spawnPosition.z == CloudGenerator.CLOUD_Z_POSITION
            ));

            _cloud.Received(expectedNumOfClouds).Initialise(_cloudStats);
        }
    }
}
