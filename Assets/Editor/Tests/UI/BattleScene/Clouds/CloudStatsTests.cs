using BattleCruisers.UI.BattleScene.Clouds;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.BattleScene.Clouds
{
    public class CloudStatsTests
    {
        private ICloudGenerationStats _generationStats;

        [SetUp]
        public void SetuUp()
        {
            _generationStats = Substitute.For<ICloudGenerationStats>();

            // -80 <= x <= 80
            //  15 <= y <= 60
            Rect spawnArea = new Rect(x: -80, y: 15, width: 160, height: 45);

            _generationStats.CloudSpawnArea.Returns(spawnArea);
        }

        [Test]
        public void PositiveCloudMovementSpeed()
        {
            _generationStats.CloudHorizontalMovementSpeedInS.Returns(3);

            ICloudStatsExtended cloudStats = new CloudStatsExtended(_generationStats);

            Assert.AreEqual(80, cloudStats.DisappearLineInM);
            Assert.AreEqual(-80, cloudStats.ReappaerLineInM);
        }

        [Test]
        public void NegativeCloudMovementSpeed()
        {
            _generationStats.CloudHorizontalMovementSpeedInS.Returns(-3);

            ICloudStatsExtended cloudStats = new CloudStatsExtended(_generationStats);

            Assert.AreEqual(-80, cloudStats.DisappearLineInM);
            Assert.AreEqual(80, cloudStats.ReappaerLineInM);
        }
    }
}
